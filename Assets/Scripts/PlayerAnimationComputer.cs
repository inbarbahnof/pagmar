public class PlayerAnimationComputer
{
    public PlayerAnimation Compute(PlayerAnimationInput input)
    {
        // TODO climb, pet, walk animations
        if (input._playerState == PlayerState.Climb) return PlayerAnimation.Climb;
        
        if (input._isCalling) return PlayerAnimation.Call;

        if (input._justPickedUp) return PlayerAnimation.PickUp;

        if (input._throwing) return PlayerAnimation.Throw;

        if (input._isAiming) return PlayerAnimation.Aim;
        
        switch (input._playerState)
        {
            case PlayerState.Walk:
                return HandleWalkAnim(input);
            case PlayerState.Stealth:
                return HandleStealthAnim(input);
            case PlayerState.Push:
                return HandlePushingAnim(input);
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
        if (!input._canMove) return PlayerAnimation.Idle;
        return PlayerAnimation.Run;
    }

    private PlayerAnimation HandlePushingAnim(PlayerAnimationInput input)
    {
        if (input._isPushingFromLeft)
        {
            if (!input._movingRight) return PlayerAnimation.Pull;
            return PlayerAnimation.Push;
        }
        
        if (!input._movingRight) return PlayerAnimation.Push;
        return PlayerAnimation.Pull;
    }
}