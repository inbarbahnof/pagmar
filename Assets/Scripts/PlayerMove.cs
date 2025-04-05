using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _speed = 6f;
    
    private Vector2 _moveInput = Vector2.zero;
    private Rigidbody2D _playerRb;

    private PlayerStateManager _stateManager;
    private bool isMoving = false;
    
    void Start()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _stateManager = GetComponent<PlayerStateManager>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 movement = _playerRb.position + _moveInput * (_speed * Time.fixedDeltaTime);
        _playerRb.MovePosition(movement);
        
        if (_moveInput != Vector2.zero)
        {
            if (!isMoving)
            {
                isMoving = true;
                _stateManager.SetState(PlayerState.Walk);
            }
        }
        else
        {
            isMoving = false;
            _stateManager.SetState(PlayerState.Idle);
        }
    }

    public void UpdateMoveInput(Vector2 moveInput)
    {
        _moveInput = moveInput;
    }
}
