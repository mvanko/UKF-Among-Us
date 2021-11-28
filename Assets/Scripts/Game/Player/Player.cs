using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private InputAction _input;

    private Vector2 _lastMovementInput;
    private PlayerData.PlayerColor _playerColor;
    private Sprite _playerSprite;

    static Color myColor;
    SpriteRenderer myAvatarSprite;

    private bool isDead = false;

    public PlayerData.PlayerColor PlayerColor => _playerColor;
    
    [SerializeField] bool isImposter;
    [SerializeField] InputAction KILL;
    
    Player target;
    [SerializeField] Collider myCollider;
    
    private void Awake()
    {
        KILL.performed += KILLTarget;
    }
    
    public void SetRole (bool newRole)
    {
        isImposter = newRole;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player tempTarget = other.GetComponent<Player>();
            if (isImposter)
            {
                if (tempTarget.isImposter)
                    return;
                else
                {
                    target = tempTarget;
                    Debug.Log(target.name);
                }
            }
        }
    }
    
    void KillTarget(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (target == null)
                return;
            else
            {
                if (target.isDead)
                    return;
                _playerTransform.position = target.transform.position;
                target.Die();
                target = null;
            }
        }
    }

    public void Die()
    {
        isDead = true; 
        
        _playerAnimator.SetBool("IsDead", isDead);
        myCollider.enabled;
    }
    
    //TODO
    public void Setup(PlayerData.PlayerColor playerColor, PlayerData playerData)
    {
        _playerColor = playerColor;
        
        switch(_playerColor)
        {
            case PlayerData.PlayerColor.BLUE:
                break;
            case PlayerData.PlayerColor.GREEN:
                break;
            case PlayerData.PlayerColor.ORANGE:
                break;
            case PlayerData.PlayerColor.PINK:
                break;
            case PlayerData.PlayerColor.RED:
                break;
            case PlayerData.PlayerColor.WHITE:
                break;
            case PlayerData.PlayerColor.YELLOW:
                break;
        }
    }

    private void OnEnable()
    {
        KILL.Enable();
        _playerAnimator.SetBool("IsDead", true); //for testing       
    }

    private void OnDisable()
    {
        KILL.Disable();
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

    private void Start()
    {
        myAvatarSprite = _playerTransform.GetComponent<SpriteRenderer>();
        if (myColor == Color.clear)
            myColor = Color.white;
        myAvatarSprite.color = myColor;
    }

    private void Update()
    {
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

    public void SetColor(Color newColor) {
        myColor = newColor;
        if (myAvatarSprite != null) 
        {
            myAvatarSprite.color = myColor;
        }
    }
}
