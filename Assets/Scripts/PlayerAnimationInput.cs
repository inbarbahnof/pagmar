using UnityEngine.Rendering.Universal;

public struct PlayerAnimationInput
{
    public PlayerState _playerState;
    public bool _isCrouching;
    public bool _isMoving;
    public bool _canMove;
    public bool _isCalling;
    public bool _movingRight;
    public bool _standing;
    public bool _didPickUp;
    public bool _justPickedUp;
    public bool _throwing;
    public bool _isPushingFromLeft;
    public bool _isAiming;
    public bool _petting;
    public bool _goingBackFromPet;
    public bool _narrowPass;
    public bool _sad;
    public bool _smoothMoving;

    public PlayerAnimationInput(PlayerState playerState, bool isCrouching, 
        bool isMoving,bool canMove, bool isCalling, bool movingRight,
        bool standing, bool didPickUp,
        bool justPickedUp, bool throwing, bool isPushingFromLeft,
        bool isAiming, bool petting, bool goingBackFromPet, bool narrowPass,
        bool sad, bool smoothMoving)
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
        _petting = petting;
        _goingBackFromPet = goingBackFromPet;
        _narrowPass = narrowPass;
        _standing = standing;
        _sad = sad;
        _smoothMoving = smoothMoving;
    }
}