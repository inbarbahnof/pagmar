using UnityEngine.Rendering.Universal;

public struct PlayerAnimationInput
{
    public PlayerState _playerState;
    public bool _isCrouching;
    public bool _isMoving;
    public bool _canMove;
    public bool _isCalling;
    public bool _movingRight;
    public bool _didPickUp;
    public bool _justPickedUp;
    public bool _throwing;
    public bool _isPushingFromLeft;
    public bool _isAiming;

    public PlayerAnimationInput(PlayerState playerState, bool isCrouching, 
        bool isMoving,bool canMove, bool isCalling, bool movingRight, bool didPickUp,
        bool justPickedUp, bool throwing, bool isPushingFromLeft,bool isAiming)
    {
        _playerState = playerState;
        _isCrouching = isCrouching;
        _isMoving = isMoving;
        _canMove = canMove;
        _isCalling = isCalling;
        _movingRight = movingRight;
        _didPickUp = didPickUp;
        _justPickedUp = justPickedUp;
        _throwing = throwing;
        _isPushingFromLeft = isPushingFromLeft;
        _isAiming = isAiming;
    }
}