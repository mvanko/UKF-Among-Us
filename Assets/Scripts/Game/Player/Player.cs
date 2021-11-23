using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody _playerRigidbody;
    [SerializeField] Transform _playerTransform;
    [SerializeField] InputAction _input;
    [SerializeField] Animator _myAnim;

    private Vector2 _movementInput;
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

    void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerTransform = transform.GetChild(0);
        _myAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        _movementInput = _input.ReadValue<Vector2>();

        //Kod na otaèanie postavy, zatia¾ nepotrebné
        if(_movementInput.x != 0)
        {
            _playerTransform.localScale = new Vector2(Mathf.Sign(_movementInput.x), 1);
        }
        _myAnim.SetFloat("Speed", _movementInput.magnitude);
    }

    private void FixedUpdate()
    {
        _playerRigidbody.velocity = _movementInput * GameManager.Instance.PlayerData.movementSpeed;
    }
}
