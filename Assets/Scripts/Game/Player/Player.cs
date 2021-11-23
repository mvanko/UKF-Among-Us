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
    private PlayerData.Color _playerColor;
    private Sprite _playerSprite;

    public PlayerData.Color PlayerColor => _playerColor;

    //TODO
    public void Setup(PlayerData.Color playerColor, PlayerData playerData)
    {
        _playerColor = playerColor;
        
        switch(_playerColor)
        {
            case PlayerData.Color.BLUE:
                break;
            case PlayerData.Color.GREEN:
                break;
            case PlayerData.Color.ORANGE:
                break;
            case PlayerData.Color.PINK:
                break;
            case PlayerData.Color.RED:
                break;
            case PlayerData.Color.WHITE:
                break;
            case PlayerData.Color.YELLOW:
                break;
        }
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
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
