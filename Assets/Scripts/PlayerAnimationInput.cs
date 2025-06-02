public struct PlayerAnimationInput
{
    public PlayerState _playerState;
    public bool _isCrouching;
    public bool _isMoving;
    public bool _isPushing;

    public PlayerAnimationInput(PlayerState playerState, bool isCrouching, 
        bool isMoving, bool isPushing)
    {
        _playerState = playerState;
        _isCrouching = isCrouching;
        _isMoving = isMoving;
        _isPushing = isPushing;
    }
}