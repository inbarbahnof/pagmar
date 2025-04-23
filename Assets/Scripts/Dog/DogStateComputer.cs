using Unity.VisualScripting.FullSerializer;

namespace Dog
{
    public class DogStateComputer
    {
        
        public DogState Compute(DogState previousDogState, DogStateMachineInput machineInput)
        {
            if (machineInput._playerState == PlayerState.Call && 
                machineInput._playerDogDistance <= machineInput._callDistance) 
                return DogState.FollowCall;
            
            if (machineInput._dogBusy) return DogState.OnTargetAction;

            if (machineInput._dogFollowingTOI) return DogState.FollowTOI;
            
            switch (machineInput._playerState)
            {
                case PlayerState.Walk:
                    return HandlePlayerWalkBehavior(previousDogState, machineInput);
                case PlayerState.Idle:
                    return HandlePlayerIdleBehavior(previousDogState, machineInput);
                case PlayerState.Push:
                    return HandlePlayerPushBehavior(previousDogState, machineInput);
            }
            
            return previousDogState;
        }
        
        private DogState HandlePlayerPushBehavior(DogState previousDogState, DogStateMachineInput machineInput)
        {
            if (machineInput._playerDogDistance <= machineInput._pushDistance)
            {
                return DogState.Push;
            }

            return previousDogState;
        }
    
        private DogState HandlePlayerWalkBehavior(DogState previousDogState, DogStateMachineInput machineInput)
        {
            if (previousDogState != DogState.FollowTOI)
            {
                if (machineInput._playerDogDistance <= machineInput._followDistance)
                {
                    return DogState.Follow;
                }

                return DogState.Idle;
            }

            return previousDogState;
        }

        private DogState HandlePlayerIdleBehavior(DogState previousDogState, DogStateMachineInput machineInput)
        {
            // Idle,
            // Follow,
            // FollowTOI,
            // Push
            
            // public bool _dogReachedTarget;
            // public bool _dogFollowingTarget;
            // public bool _dogFollowingTOI;

            if (machineInput._dogReachedTarget)
            {
                return DogState.Idle;
            }
            
            return DogState.Follow;
        }
    }
}