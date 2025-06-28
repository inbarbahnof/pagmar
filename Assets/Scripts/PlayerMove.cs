using System.Collections;
using Audio.FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _runSpeed = 4.7f;
    [SerializeField] private float _runFastSpeed = 6f;
    [SerializeField] private float _crouchSpeed = 3f;
    [SerializeField] private float _narrowPassSpeed = 4f;
    [SerializeField] private float _pushSpeed = 3.7f;
    [SerializeField] private GameObject _playerArt;

    private Vector2 _moveInput = Vector2.zero;
    private Rigidbody2D _playerRb;
    private PlayerStateManager _stateManager;
    private float _speed;
    private Vector2 _aimInput;

    private bool isMoving = false;
    private bool isPushing = false;
    private bool canMove = true;
    private bool movingRight = true;
    private bool _standing;
    private bool _crouching;
    
    private bool isAutoRunning = false;
    private Vector2 autoRunDirection = new Vector2(1f, -0.6f);
    private float verticalInputFactor = 0.5f; 
    private Coroutine autoRunStopCoroutine;

    public bool IsMoving => isMoving;
    public bool CanMove => canMove;
    public bool MovingRight => movingRight;
    public bool Standing => _standing;
    public bool Pushing => isPushing;
    public Vector2 MoveInput => _moveInput;
    public Vector2 AimInput => _aimInput;

    void Start()
    {
        _speed = _runSpeed;
        
        _playerRb = GetComponent<Rigidbody2D>();
        _stateManager = GetComponent<PlayerStateManager>();
    }
    
    public void StopAutoRun(float duration = 1f)
    {
        if (autoRunStopCoroutine != null)
        {
            StopCoroutine(autoRunStopCoroutine);
        }
        autoRunStopCoroutine = StartCoroutine(GradualStopAutoRun(duration));
    }
    
    private IEnumerator GradualStopAutoRun(float duration)
    {
        Vector2 startInput = _moveInput;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            _moveInput = Vector2.Lerp(startInput, Vector2.zero, t);
            yield return null;
        }

        _moveInput = Vector2.zero;
        isAutoRunning = false;
        UpdatePlayerRunning(false);
    }

    public void UpdatePlayerRunning(bool isRunning)
    {
        if (isRunning)
        {
            _speed = _runFastSpeed;
            _stateManager.UpdatePlayerSpeed(true);
        }
        else
        {
            _speed = _runSpeed;
            _stateManager.UpdatePlayerSpeed(false);
        }
    }

    public void UpdateNarrowPass(bool narrow)
    {
        if (narrow) _speed = _narrowPassSpeed;
        else if (_crouching) _speed = _crouchSpeed;
        else _speed = _runSpeed;
    }

    public void UpdateCrouch(bool crouch)
    {
        if (crouch)
        {
            _speed = _crouchSpeed;
            _crouching = true;
        }
        else
        {
            _speed = _runSpeed;
            _crouching = false;
        }
    }
    
    public void StartAutoRunWithVerticalControl()
    {
        canMove = true;
        isAutoRunning = true;
        
        _moveInput = autoRunDirection;
        
        UpdatePlayerRunning(true);
    }

    private void FixedUpdate()
    {
        if (canMove) Move();
        
        movingRight = _moveInput.x > 0;
        _standing = _moveInput == Vector2.zero;
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

    public void UpdateAimInput(Vector2 input)
    {
        _aimInput = input;
    }
    
    public void CheckIfNeedToFlipArt()
    {
        Vector3 scale = _playerArt.transform.localScale;
        bool pushingFromLeft = _stateManager.IsPushingFromLeft;
        bool facingRight = scale.x >= 0;

        // If pushing from the left, player should face left (scale.x < 0)
        if (pushingFromLeft && !facingRight)
        {
            scale.x = Mathf.Abs(scale.x);
            _playerArt.transform.localScale = scale;
        }
        // If pushing from the right, player should face right (scale.x > 0)
        else if (!pushingFromLeft && facingRight)
        {
            scale.x = -Mathf.Abs(scale.x);
            _playerArt.transform.localScale = scale;
        }
    }

    public void SetIsPushing(bool push, Vector3 playerPos, bool stationary = false)
    {
        isPushing = push;

        if (isPushing)
        {
            if (stationary) _playerRb.constraints |= RigidbodyConstraints2D.FreezePositionX;
            else _playerRb.constraints |= RigidbodyConstraints2D.FreezePositionY;
            GetComponent<SmoothMover>().MoveTo(playerPos);

            _speed = _pushSpeed;

        }
        else
        {
            _playerRb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            _playerRb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;

            _speed = _runSpeed;
        }
    }

    public float GetReadyToClimb(Vector2 playerPos, bool climb = false)
    {
        float waitTime = GetComponent<SmoothMover>().MoveTo(playerPos);
        if (climb) _playerRb.constraints |= RigidbodyConstraints2D.FreezePositionY;
        return waitTime;
    }
    
    public void FinishClimb()
    {
        _playerRb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
    }


    public void SetCanMove(bool move)
    {
        canMove = move;
        // if (move) _stateManager.SetIdleState();
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
        if (isAutoRunning)
        {
            // Only allow vertical influence (add slight control to the base autoRunDirection)
            _moveInput = new Vector2(autoRunDirection.x, autoRunDirection.y + moveInput.y * verticalInputFactor);
        }
        else
        {
            _moveInput = moveInput;
        }
    }

    public void ResetToCheckpoint(Vector2 pos)
    {
        transform.position = pos;
        isMoving = false;
        UpdateMoveInput(Vector2.zero);
        SetIsPushing(false, Vector3.zero);
        SetIsSwinging(false);
        _stateManager.OnFinishedInteraction();
    }
}
