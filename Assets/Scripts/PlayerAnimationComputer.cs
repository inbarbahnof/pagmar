public class PlayerAnimationComputer
{
    public PlayerAnimation Compute(PlayerAnimationInput input)
    {
        if (input._isCalling) return PlayerAnimation.Call;

        if (input._justPickedUp) return PlayerAnimation.PickUp;
        
        switch (input._playerState)
        {
            case PlayerState.Walk:
                return HandleWalkAnim(input);
            case PlayerState.Stealth:
                return HandleStealthAnim(input);
            case PlayerState.Push:
                return HandlePushingAnim(input);
            case PlayerState.Throw:
                return PlayerAnimation.Throw;
            
            // case PlayerState.Pet:
            //     return PlayerAnimation.Pet;
        }
        
        
        return PlayerAnimation.Idle;
    }

    private PlayerAnimation HandleStealthAnim(PlayerAnimationInput input)
    {
        if (input._isMoving) return PlayerAnimation.CrouchWalk;
        
        return PlayerAnimation.CrouchIdle;
    }

    private PlayerAnimation HandleWalkAnim(PlayerAnimationInput input)
    {
        if (input._isCrouching) return PlayerAnimation.CrouchWalk;
        return PlayerAnimation.Run;
    }

    private PlayerAnimation HandlePushingAnim(PlayerAnimationInput input)
    {
        if (!input._movingRight) return PlayerAnimation.Pull;
        
        return PlayerAnimation.Push;
    }
}