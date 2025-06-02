using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _speed = 6f;
    [SerializeField] private GameObject _playerArt;

    private Vector2 _moveInput = Vector2.zero;
    private Rigidbody2D _playerRb;
    private PlayerStateManager _stateManager;

    private bool isMoving = false;
    private bool isPushing = false;
    private bool canMove = true;
    private bool movingRight = true;

    private Vector3 _lastPosition;

    public bool IsMoving => isMoving;
    public bool IsPushing => isPushing;
    public bool MovingRight => movingRight;

    void Start()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _stateManager = GetComponent<PlayerStateManager>();
        _lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (canMove) Move();
        FlipSpriteBasedOnMovement();
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
            }
        }
        else
        {
            isMoving = false;
        }

        if (!isPushing)
        {
            _stateManager.UpdateWalking(isMoving);
        }
    }

    private void FlipSpriteBasedOnMovement()
    {
        if (Mathf.Abs(_moveInput.x) > 0.001f && !isPushing)
        {
            if (!isPushing)
            {
                float newScaleX = _moveInput.x > 0 ? 1 : -1;
                _playerArt.transform.localScale = new Vector3(
                    newScaleX * Mathf.Abs(_playerArt.transform.localScale.x),
                    _playerArt.transform.localScale.y,
                    _playerArt.transform.localScale.z
                );
            }
        }
        movingRight = _moveInput.x > 0;
        // if (!isPushing)
        // {
        //     _lastPosition = currentPosition;
        // }
    }

    public void SetIsPushing(bool push, Vector3 playerPos, bool stationary = false)
    {
        isPushing = push;

        if (isPushing)
        {
            if (stationary) _playerRb.constraints |= RigidbodyConstraints2D.FreezePositionX;
            else _playerRb.constraints |= RigidbodyConstraints2D.FreezePositionY;
            GetComponent<SmoothMover>().MoveTo(playerPos);
        }
        else
        {
            _playerRb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            _playerRb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        }
    }

    public void SetCanMove(bool move)
    {
        canMove = move;
    }

    public void SetIsSwinging(bool swing)
    {
        canMove = !isMoving;
    }

    public bool GetMoveDirRight()
    {
        return movingRight;
    }

    public void UpdateMoveInput(Vector2 moveInput)
    {
        _moveInput = moveInput;
    }

    public void ResetToCheckpoint(Vector2 pos)
    {
        transform.position = pos;
        isMoving = false;
        UpdateMoveInput(Vector2.zero);
        SetIsPushing(false, Vector3.zero);
        SetIsSwinging(false);
    }
}
