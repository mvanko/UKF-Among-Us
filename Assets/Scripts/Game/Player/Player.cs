using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IPunObservable
{
    [SerializeField] private GameObject _deadBodyPrototype;
    [SerializeField] private GameObject _otherPlayer;

    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private SpriteRenderer _playerSpriteRenderer;
    [SerializeField] private Collider _playerCollider;

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

    public static List<Transform> allBodies;
    List<Transform> bodiesFound;

    [SerializeField] InputAction REPORT;
    [SerializeField] LayerMask ignoreForBody;

    [SerializeField] Camera myCamera;
    [SerializeField] GameObject lightMask;

    PhotonView _PV;


    private void Awake()
    {
        KILL.performed += KillTarget;

        REPORT.performed += ReportBody;

    }

    private void Start()
    {
        _PV = GetComponent<PhotonView>();

        if (myColor == Color.clear)
            myColor = Color.white;

        if(_PV.IsMine)
        {
            _localPlayer = this;
        }
        else
        {
            myCamera.gameObject.SetActive(false);
            lightMask.SetActive(false);
            return;
        }

        _playerSpriteRenderer.color = myColor;

        allBodies = new List<Transform>();
        bodiesFound = new List<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player tempTarget = other.GetComponent<Player>();
            if (_isImposter)
            {
                if (tempTarget._isImposter)
                    return;
                else
                {
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
            if(targets.Contains(tempTarget))
            {
                targets.Remove(tempTarget);
            }
        }
    }

    private void OnEnable()
    {
        _input.Enable();
        KILL.Enable();

        REPORT.Enable();

    }

    private void OnDisable()
    {
        _input.Disable();
        KILL.Disable();

        REPORT.Disable();

    }

    public void SetRole(bool newRole)
    {
        _isImposter = newRole;
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
                target.Die();
                targets.Remove(target);
            }
        }
    }

    public void Die()
    {
        isDead = true;
        _playerAnimator.SetBool("IsDead", isDead);
        _playerCollider.enabled = false;
        gameObject.layer = 9;

        DeadBody deadBody = Instantiate(_deadBodyPrototype.transform, transform.position, transform.rotation).GetComponent<DeadBody>();
        deadBody.Setup(_playerSpriteRenderer.color);
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
        else if(newMovementInput.magnitude == 0)
        {
            _playerAnimator.SetBool("Idle", true);
        }
    }

    private void Update()
    {

        _playerTransform.localScale = new Vector2(direction, 1);

        if (!_PV.IsMine)
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

        if(allBodies.Count > 0)
        {
          BodySearch();
        }

    }

    private void FixedUpdate()
    {
        if (!_PV.IsMine)
        {
            return;
        }
        if (SceneManager.GetActiveScene().name == "Waiting Room")
        {
            return;
        }

        _playerRigidbody.velocity = _lastMovementInput * GameManager.Instance.PlayerData.movementSpeed;

    }

    void BodySearch()
    {
      foreach(Transform body in allBodies)
      {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, body.position - transform.position);
        Debug.DrawRay(transform.position, body.position - transform.position, Color.cyan);
        if(Physics.Raycast(ray, out hit, 1000f, ~ignoreForBody))
        {
          if(hit.transform == body)
          {
            if(bodiesFound.Contains(body.transform))
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
      }
    }

    private void ReportBody(InputAction.CallbackContext obj)
    {
      if(bodiesFound == null)
      {
        return;
      }
      if(bodiesFound.Count == 0)
      {
        return;
      }
      Transform tempBody = bodiesFound[bodiesFound.Count -1];
      allBodies.Remove(tempBody);
      bodiesFound.Remove(tempBody);
      tempBody.GetComponent<DeadBody>().Report();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(direction);
        }
        else
        {
            direction = (float)stream.ReceiveNext();
        }
    }
}
