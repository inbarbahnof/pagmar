using UnityEngine.Rendering.Universal;

public struct PlayerAnimationInput
{
    public PlayerState _playerState;
    public bool _isCrouching;
    public bool _isMoving;
    public bool _isCalling;
    public bool _movingRight;
    public bool _didPickUp;
    public bool _justPickedUp;

    public PlayerAnimationInput(PlayerState playerState, bool isCrouching, 
        bool isMoving, bool isCalling, bool movingRight, bool didPickUp,
        bool justPickedUp)
    {
        _playerState = playerState;
        _isCrouching = isCrouching;
        _isMoving = isMoving;
        _isCalling = isCalling;
        _movingRight = movingRight;
        _didPickUp = didPickUp;
        _justPickedUp = justPickedUp;
    }
}