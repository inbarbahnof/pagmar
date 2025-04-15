using UnityEngine;

namespace DefaultNamespace
{
    public struct DogStateMachineInput
    {
        public PlayerState _playerState;
        public float _playerDogDistance;
        
        public bool _dogReachedTarget;
        public bool _dogFollowingTarget;
        public bool _dogFollowingTOI;

        public float _pushDistance;

        public DogStateMachineInput(PlayerState playerState, float playerDogDistance, bool dogReachedTarget, bool dogFollowingTarget, bool dogFollowingTOI, float pushDistance)
        {
            _playerState = playerState;
            _playerDogDistance = playerDogDistance;
            _dogReachedTarget = dogReachedTarget;
            _dogFollowingTarget = dogFollowingTarget;
            _dogFollowingTOI = dogFollowingTOI;
            _pushDistance = pushDistance;
        }
    }
}