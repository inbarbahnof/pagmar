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

    private Vector3 _lastPosition;

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
                if (!isPushing) _stateManager.SetState(PlayerState.Walk);
            }
        }
        else
        {
            isMoving = false;
            if (!isPushing) _stateManager.SetState(PlayerState.Idle);
        }
    }

    private void FlipSpriteBasedOnMovement()
    {
        Vector3 currentPosition = transform.position;
        float dirX = currentPosition.x - _lastPosition.x;

        if (Mathf.Abs(dirX) > 0.001f && !isPushing)
        {
            float newScaleX = dirX > 0 ? 1 : -1;
            _playerArt.transform.localScale = new Vector3(
                newScaleX * Mathf.Abs(_playerArt.transform.localScale.x),
                _playerArt.transform.localScale.y,
                _playerArt.transform.localScale.z
            );
        }

        if (!isPushing)
        {
            _lastPosition = currentPosition;
        }
    }

    public void SetIsPushing(bool push, Vector3 playerPos)
    {
        isPushing = push;

        if (isPushing)
        {
            _playerRb.constraints |= RigidbodyConstraints2D.FreezePositionY;
            transform.position = playerPos;
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

    public void ResetToCheckpoint(Vector2 pos)
    {
        transform.position = pos;
        isMoving = false;
        UpdateMoveInput(Vector2.zero);
        SetIsPushing(false, Vector3.zero);
        SetIsSwinging(false);
    }
}
