using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _speed = 6f;
    
    private Vector2 _moveInput = Vector2.zero;
    private Vector2 _prevMoveInput;
    private Rigidbody2D _playerRb;

    private PlayerStateManager _stateManager;
    private bool isMoving = false;
    private bool isPushing = false;
    private bool canMove = true;
    
    void Start()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _stateManager = GetComponent<PlayerStateManager>();
    }

    private void FixedUpdate()
    {
        if (canMove) Move();
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
                
                if (!isPushing) _stateManager.SetState(PlayerState.Walk);
            }

            if (!isPushing && Mathf.Sign(_moveInput.x) != Mathf.Sign(_prevMoveInput.x))
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }

            if (!isPushing)
            {
                _prevMoveInput = _moveInput;
            }
        }
        else
        {
            isMoving = false;
            if (!isPushing) _stateManager.SetState(PlayerState.Idle);
        }
    }

    public void SetIsPushing(bool push)
    {
        isPushing = push;

        if (isPushing)
        {
            _playerRb.constraints |= RigidbodyConstraints2D.FreezePositionY;
        }
        else
        {
            _playerRb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        }
    }

    public void SetIsSwinging(bool swing)
    {
        canMove = !isMoving;
    }

    public void UpdateMoveInput(Vector2 moveInput)
    {
        _moveInput = moveInput;
    }
}
