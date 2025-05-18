public struct PlayerAnimationInput
{
    public PlayerState _playerState;
    public bool _isCrouching;

    public PlayerAnimationInput(PlayerState playerState, bool isCrouching)
    {
        _playerState = playerState;
        _isCrouching = isCrouching;
    }
}