using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private SpriteRenderer _playerSpriteRenderer;
    [SerializeField] private Collider _playerCollider;

    [SerializeField] private bool _isImposter;
    [SerializeField] private bool _hasControl;

    [SerializeField] private InputAction _input;
    [SerializeField] private InputAction KILL;

    private static Player _localPlayer;
    static Color myColor;

    private Vector2 _lastMovementInput;
    private bool isDead = false;

    private Player tempTarget = null;
    public static Player LocalPlayer => _localPlayer;
    
    private void Awake()
    {
        KILL.performed += KillTarget;
    }

    private void Start()
    {
        if (myColor == Color.clear)
            myColor = Color.white;

        if(_hasControl)
        {
            _localPlayer = this;
        } 
        else
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
                if (tempTarget._isImposter)
                    return;
                else
                {
                    this.tempTarget = tempTarget;
                    Debug.Log(this.tempTarget.name);
                }
            }
        }
    }

    private void OnEnable()
    {
        _input.Enable();
        KILL.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
        KILL.Disable();
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
            if (tempTarget == null)
                return;
            else
            {
                if (tempTarget.isDead)
                    return;
                _playerTransform.position = tempTarget._playerTransform.position;
                tempTarget.Die();
                tempTarget = null;
            }
        }
    }

    public void Die()
    {
        isDead = true; 
        
        _playerAnimator.SetBool("IsDead", isDead);
        _playerCollider.enabled = false;
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
        if(!_hasControl)
        {
            return;
        }

        Vector2 currentInput = _input.ReadValue<Vector2>();

        if (_lastMovementInput != currentInput)
        {
            ChangeAnimation(currentInput);
        }

        _lastMovementInput = currentInput;
    }

    private void FixedUpdate()
    {
        _playerRigidbody.velocity = _lastMovementInput * GameManager.Instance.PlayerData.movementSpeed;
    }
}
