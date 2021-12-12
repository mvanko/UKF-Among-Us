using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class Player : MonoBehaviour, IPunObservable
{
    [SerializeField] private GameObject _deadBodyPrototype;
    [SerializeField] private GameObject _otherPlayer;

    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private SpriteRenderer _playerSpriteRenderer;
    [SerializeField] private Collider _playerCollider;

    [SerializeField] Text _playerNameText;

    [SerializeField] private bool _isImposter;

    [SerializeField] private InputAction _input;
    [SerializeField] private InputAction KILL;

    public static Player _localPlayer;
    static Color myColor;

    float direction = 1;

    private Vector2 _lastMovementInput;
    private bool isDead = false;

    private List<Player> targets = new List<Player>();

    public static Player LocalPlayer => _localPlayer;

    public static List<Transform> allBodies = new List<Transform>();
    List<Transform> bodiesFound = new List<Transform>();

    [SerializeField] InputAction INTERACTION;
    [SerializeField] InputAction MOUSE;
    [SerializeField] InputAction REPORT;
    [SerializeField] LayerMask ignoreForBody;
    [SerializeField] LayerMask interactLayer;

    [SerializeField] LayerMask everythingMask;

    [SerializeField] Camera myCamera;
    [SerializeField] GameObject lightMask;

    private Vector2 mousePositionInput;
    PhotonView _PV;

    private InteractableObject highlightedInteractableObject;
    private InteractableObject activeInteractableObject;
    private bool killAvailable = false;
    private bool reportAvailable = false;
    private bool useAvailable = false;

    private float killCooldown = 30f;

    public float KillCooldown => killCooldown;

    public bool IsDead => isDead;
    public bool IsImposter => _isImposter;
    public bool KillAvailable => killAvailable;
    public bool UseAvailable => useAvailable;
    public bool ReportAvailable => reportAvailable;

    public static event Action OnPlayerReady;
    public static event Action OnReportStarted;

    public event Action OnPlayerUpdated;
    public event Action<bool> OnKillAvailable;
    public event Action<bool> OnReportAvailable;
    public event Action<bool> OnUseAvailable;
    
    public event Action<Minigame> OnMinigameWon;

    private void Awake()
    {
        KILL.performed += KillTarget;
        REPORT.performed += ReportBody;
        INTERACTION.performed += Interact;

        _PV = GetComponent<PhotonView>();
    }

    void OnDestroy()
    {
        if (this == LocalPlayer)
        {
            InteractableObject.OnHighlighted -= UpdateInteractableHighlighted;
        }
    }

    private void Start()
    {
        if (_PV != null && _PV.IsMine)
        {
            _localPlayer = this;
            InteractableObject.OnHighlighted += UpdateInteractableHighlighted;
            OnPlayerReady?.Invoke();
        }

        if (SceneManager.GetActiveScene().name == "Waiting Room")
        {
            _playerNameText.enabled = false;
            if (_PV != null && !_PV.IsMine)
            {
                _otherPlayer.gameObject.SetActive(false);
            }
        }

        if (_PV.IsMine)
        {
            _playerNameText.text = PhotonNetwork.NickName;
        }
        else
        {
            _playerNameText.text = _PV.Owner.NickName;
        }

        if (myColor == Color.clear)
            myColor = Color.white;

        if (_PV != null && !_PV.IsMine)
        {
            myCamera.gameObject.SetActive(false);
            lightMask.SetActive(false);
        }

        if (_PV != null && !_PV.IsMine)
        {
            return;
        }
        _playerSpriteRenderer.color = myColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player tempTarget = other.GetComponent<Player>();
            if (_isImposter)
            {
                if (tempTarget._isImposter || tempTarget.IsDead)
                    return;
                else
                {
                    if (this == _localPlayer && !killAvailable && killCooldown <= 0f)
                    {
                        killAvailable = true;
                        OnKillAvailable?.Invoke(true);
                    }

                    targets.Add(tempTarget);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Player tempTarget = other.GetComponent<Player>();
            if (targets.Contains(tempTarget))
            {
                targets.Remove(tempTarget);
            }

            if (this == _localPlayer && targets.Count == 0 && killAvailable && !tempTarget.IsDead)
            {
                killAvailable = false;
                OnKillAvailable?.Invoke(false);
            }
        }
    }

    private void OnEnable()
    {
        _input.Enable();
        KILL.Enable();
        REPORT.Enable();
        MOUSE.Enable();
        INTERACTION.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
        KILL.Disable();
        REPORT.Disable();
        MOUSE.Disable();
        INTERACTION.Disable();
    }

    public void SetRole(int bullyNo1, int bullyNo2, int bullyNo3)
    {
        if (bullyNo2 == -1 && bullyNo3 == -1)
        {
            if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[bullyNo1])
            {
                _isImposter = true;
            }
        }
        else if (bullyNo3 == -1)
        {
            if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[bullyNo1] || PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[bullyNo2])
            {
                _isImposter = true;
            }
        }
        else
        {
            if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[bullyNo1] || PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[bullyNo2] || PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[bullyNo3])
            {
                _isImposter = true;
            }
        }

        OnPlayerUpdated?.Invoke();
    }

    public void SetColor(Color newColor)
    {
        myColor = newColor;
        if (_playerSpriteRenderer != null)
        {
            _playerSpriteRenderer.color = myColor;
        }
    }

    void KillTarget(InputAction.CallbackContext context)
    {

        if (!_PV.IsMine || !_isImposter)
        {
            return;
        }

        if (context.phase == InputActionPhase.Performed)
        {
            if (targets.Count == 0)
                return;
            else
            {
                Player target = targets[targets.Count - 1];
                if (target.isDead)
                    return;
                _playerTransform.position = target._playerTransform.position;
                target._PV.RPC("RPC_Kill", RpcTarget.All);
                targets.Remove(target);

                killCooldown = 20f;
                OnKillAvailable?.Invoke(false);
            }
        }
    }

    [PunRPC]
    void RPC_Kill()
    {
        Die();
    }

    public void Die()
    {
        isDead = true;
        gameObject.layer = 9;
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = 9;
        }
        _playerCollider.enabled = false;

        if (!_PV.IsMine)
        {
            return;
        }

        _playerAnimator.SetBool("IsDead", isDead);

        myCamera.cullingMask = everythingMask;

        DeadBody deadBody = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "DeadBody"), transform.position, transform.rotation).GetComponent<DeadBody>();
        deadBody.Setup(_playerSpriteRenderer.color);

        OnPlayerUpdated?.Invoke();
    }

    private void ResetAnimatorVariables()
    {
        _playerAnimator.SetBool("Backwards", false);
        _playerAnimator.SetBool("Forwards", false);
        _playerAnimator.SetBool("Left", false);
        _playerAnimator.SetBool("Right", false);
        _playerAnimator.SetBool("Idle", false);
    }

    private void ChangeAnimation(Vector2 newMovementInput)
    {
        ResetAnimatorVariables();

        if (newMovementInput.x != 0)
        {
            if (newMovementInput.x > 0)
            {
                _playerAnimator.SetBool("Right", true);
            }
            else
            {
                _playerAnimator.SetBool("Left", true);
            }
        }
        else if (newMovementInput.y != 0)
        {
            if (newMovementInput.y > 0)
            {
                _playerAnimator.SetBool("Forwards", true);
            }
            else
            {
                _playerAnimator.SetBool("Backwards", true);
            }
        }
        else if (newMovementInput.magnitude == 0)
        {
            _playerAnimator.SetBool("Idle", true);
        }
    }

    private void Update()
    {

        if (_PV != null && !_PV.IsMine || activeInteractableObject != null)
        {
            return;
        }

        Vector2 currentInput = _input.ReadValue<Vector2>();

        if (_lastMovementInput != currentInput)
        {
            ChangeAnimation(currentInput);
        }

        _lastMovementInput = currentInput;

        if (currentInput.x != 0)
        {
            direction = Mathf.Sign(currentInput.x);
        }

        if (allBodies.Count > 0)
        {
            BodySearch();
        }

        mousePositionInput = MOUSE.ReadValue<Vector2>();

        if(_isImposter && killCooldown > 0)
        {
            killCooldown -= Time.deltaTime;
        } 
    }

    private void FixedUpdate()
    {
        if (_PV != null && !_PV.IsMine || SceneManager.GetActiveScene().name == "Waiting Room")
        {
            return;
        }

        if (activeInteractableObject != null)
        {
            _playerRigidbody.velocity = Vector3.zero;
            return;
        }

        _playerRigidbody.velocity = _lastMovementInput * GameManager.Instance.PlayerData.movementSpeed;

    }

    void BodySearch()
    {
        if (_PV.IsMine && !isDead && !reportAvailable && bodiesFound.Count > 0)
        {
            reportAvailable = true;
            OnReportAvailable?.Invoke(true);
        }
        else if (_PV.IsMine && !isDead && reportAvailable && bodiesFound.Count == 0)
        {
            reportAvailable = false;
            OnReportAvailable?.Invoke(false);
        }

        foreach (Transform body in allBodies)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, body.position - transform.position);
            Debug.DrawRay(transform.position, body.position - transform.position, Color.cyan);
            if (Physics.Raycast(ray, out hit, 5f, ~ignoreForBody))
            {
                if (hit.transform == body)
                {
                    if (bodiesFound.Contains(body.transform))
                    {
                        return;
                    }
                    bodiesFound.Add(body.transform);
                }
                else
                {
                    bodiesFound.Remove(body.transform);
                }
            }
            else
            {
                bodiesFound.Remove(body.transform);
            }
        }
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            MakeInteraction();
        }
    }

    public void MakeInteraction(bool ignoreMouseClick = false)
    {
        if (!IsImposter && highlightedInteractableObject != null && activeInteractableObject == null)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame || ignoreMouseClick)
            {
                activeInteractableObject = highlightedInteractableObject;
                activeInteractableObject.PlayMiniGame(this);
            }
            else
            {
                RaycastHit hit;
                Ray ray = myCamera.ScreenPointToRay(mousePositionInput);
                if (Physics.Raycast(ray, out hit, interactLayer))
                {
                    if (hit.transform.tag == "Interactable")
                    {
                        activeInteractableObject = highlightedInteractableObject;
                        activeInteractableObject.PlayMiniGame(this);
                    }
                }
            }
        }
    }

    private void UpdateInteractableHighlighted(InteractableObject interactableObject)
    {
        highlightedInteractableObject = interactableObject;
        useAvailable = highlightedInteractableObject != null;
        OnUseAvailable?.Invoke(useAvailable);
    }

    public void MiniGameClosed()
    {
        activeInteractableObject = null;
    }

    public void MiniGameWon(Minigame minigame)
    {
        activeInteractableObject = null;
        UpdateInteractableHighlighted(null);
        OnMinigameWon?.Invoke(minigame);
        //TODO
    }

    private void ReportBody(InputAction.CallbackContext obj)
    {
        if (bodiesFound == null || isDead)
        {
            return;
        }
        if (bodiesFound.Count == 0)
        {
            return;
        }
        Transform tempBody = bodiesFound[bodiesFound.Count - 1];
        allBodies.Remove(tempBody);
        bodiesFound.Remove(tempBody);
        tempBody.GetComponent<DeadBody>().Report();
        _PV.RPC("BodyFound", RpcTarget.All);
    }

    [PunRPC]
    private void BodyFound()
    {
        _playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        OnReportStarted?.Invoke();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(direction);
            stream.SendNext(_isImposter);
        }
        else
        {
            direction = (float)stream.ReceiveNext();
            this._isImposter = (bool)stream.ReceiveNext();
        }
    }

}
