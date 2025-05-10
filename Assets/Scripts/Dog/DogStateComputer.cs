using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Dog
{
    public class DogStateComputer
    {
        
        public DogState Compute(DogState previousDogState, DogStateMachineInput machineInput)
        {
            if (machineInput._wantsFood) return DogState.WantFood;
            
            if (machineInput._isFoodClose && machineInput._canEatFood) return DogState.FollowFood;

            if (machineInput._playerState == PlayerState.Throw || machineInput._isFollowingStick)
                return DogState.FollowStick;
            
            if (machineInput._playerState == PlayerState.Call && 
                machineInput._playerDogDistance <= machineInput._listenDistance) 
                return DogState.FollowCall;
            
            if (machineInput._dogBusy) return DogState.OnTargetAction;

            if (machineInput._dogFollowingTOI) return DogState.FollowTOI;

            if (machineInput._playerDogDistance > machineInput._listenDistance) return DogState.Idle;
            
            switch (machineInput._playerState)
            {
                case PlayerState.Walk:
                    return HandlePlayerWalkBehavior(previousDogState, machineInput);
                case PlayerState.Idle:
                    return HandlePlayerIdleBehavior(previousDogState, machineInput);
                case PlayerState.Push:
                    return HandlePlayerPushBehavior(previousDogState, machineInput);
                case PlayerState.Pet:
                    return HandlePlayerPetBehavior(previousDogState, machineInput);
            }
            
            return previousDogState;
        }

        private DogState HandleThrowStickBehavior(DogState previousDogState, DogStateMachineInput machineInput)
        {
            if (machineInput._connectionStage == 1)
            {
                if (Random.value < 0.5f) return DogState.FollowStick;
                
                return previousDogState;
            }
            return DogState.FollowStick;
        }

        private DogState HandlePlayerPetBehavior(DogState previousDogState, DogStateMachineInput machineInput)
        {
            if (machineInput._playerDogDistance <= machineInput._petDistance)
            {
                if (machineInput._connectionStage == 1)
                {
                    return DogState.GetAwayFromPlayer;
                }
                return DogState.Pet;
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
            if (machineInput._connectionStage > 1)
            {
                if (previousDogState != DogState.FollowTOI)
                {
                    return DogState.Follow;
                }

                return previousDogState;
            }

            return DogState.Idle;
        }

        private DogState HandlePlayerIdleBehavior(DogState previousDogState, DogStateMachineInput machineInput)
        {
            if (machineInput._dogReachedTarget)
            {
                return DogState.Idle;
            }
            
            return DogState.Follow;
        }
    }
}