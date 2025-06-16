using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Dog
{
    public class DogStateComputer
    {
        
        public DogState Compute(DogState previousDogState, DogStateMachineInput machineInput)
        {
            if (machineInput._wantsFood) return DogState.WantFood;
            
            if (machineInput._needToStealth ||
                machineInput is { _playerState: PlayerState.Stealth, _isStealthTargetClose: true }) 
                return DogState.Stealth;
            
            if (machineInput is { _isFoodClose: true, _canEatFood: true }) return DogState.FollowFood;

            if (machineInput._playerState == PlayerState.Throw || machineInput._isFollowingStick)
                return DogState.FollowStick;
            
            if (machineInput._dogBusy && machineInput._connectionStage <= 2) return DogState.OnTargetAction;
            
            if (previousDogState == DogState.ChaseGhostie && 
                machineInput is { _isThereGhostie: true , _dogReachedTarget: false})
                return previousDogState;

            if (machineInput._dogFollowingTOI) return DogState.FollowTOI;

            if (machineInput._playerState == PlayerState.Call &&
                machineInput._playerDogDistance <= machineInput._listenDistance)
                return HandleCallBehavior(machineInput);

            if (machineInput._followingCall) return DogState.FollowCall;
            
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

        private DogState HandleCallBehavior(DogStateMachineInput input)
        {
            if (input._isThereGhostie)
            {
                if ((input._connectionStage == 1 && input._chaseGhostieNumber < 1) ||
                (input._connectionStage > 1 && input._chaseGhostieNumber < 10))
                {
                    return DogState.ChaseGhostie;
                }
            }
            
            return DogState.FollowCall;
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
            // return DogState.Push;
            if (machineInput._followingCall)
                return DogState.Push;
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
            
            if (machineInput._followingCall)
                return DogState.Follow;

            return DogState.Idle;
        }

        private DogState HandlePlayerIdleBehavior(DogState previousDogState, DogStateMachineInput machineInput)
        {
            if (machineInput._dogReachedTarget && machineInput._connectionStage < 3)
            {
                return DogState.Idle;
            }

            if (previousDogState == DogState.ChaseGhostie)
            {
                return previousDogState;
            }
            
            return DogState.Follow;
        }
    }
}