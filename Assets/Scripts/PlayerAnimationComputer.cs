public class PlayerAnimationComputer
{
    public PlayerAnimation Compute(PlayerAnimationInput input)
    {
        switch (input._playerState)
        {
            case PlayerState.Call:
                return PlayerAnimation.Call;
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
        // TODO check whethere we need to pull or push
        return PlayerAnimation.Push;
    }
}