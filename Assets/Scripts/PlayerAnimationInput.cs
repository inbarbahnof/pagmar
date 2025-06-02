using UnityEngine.Rendering.Universal;

public struct PlayerAnimationInput
{
    public PlayerState _playerState;
    public bool _isCrouching;
    public bool _isMoving;
    public bool _isCalling;
    public bool _movingRight;

    public PlayerAnimationInput(PlayerState playerState, bool isCrouching, 
        bool isMoving, bool isCalling, bool movingRight)
    {
        _playerState = playerState;
        _isCrouching = isCrouching;
        _isMoving = isMoving;
        _isCalling = isCalling;
        _movingRight = movingRight;
    }
}