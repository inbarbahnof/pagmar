using UnityEngine;

namespace Dog
{
    public struct DogStateMachineInput
    {
        public PlayerState _playerState;
        public float _playerDogDistance;
        public int _connectionStage;
        
        public bool _dogReachedTarget;
        public bool _dogFollowingTarget;
        public bool _dogFollowingTOI;
        public bool _dogBusy;
        public bool _isFoodClose;
        public bool _isFollowingStick;

        public float _pushDistance;
        public float _listenDistance;
        public float _petDistance;

        public DogStateMachineInput(PlayerState playerState, int connectionStage,
            float playerDogDistance, bool dogReachedTarget, bool dogFollowingTarget, 
            bool dogFollowingTOI, float pushDistance, bool dogBusy,
            float listenDistance, bool isFoodClose, float petDistance, bool isFollowingStick)
        {
            _playerState = playerState;
            _playerDogDistance = playerDogDistance;
            _dogReachedTarget = dogReachedTarget;
            _dogFollowingTarget = dogFollowingTarget;
            _dogFollowingTOI = dogFollowingTOI;
            _pushDistance = pushDistance;
            _dogBusy = dogBusy;
            _listenDistance = listenDistance;
            _isFoodClose = isFoodClose;
            _connectionStage = connectionStage;
            _petDistance = petDistance;
            _isFollowingStick = isFollowingStick;
        }
    }
}