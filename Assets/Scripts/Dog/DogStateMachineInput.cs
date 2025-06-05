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
        public bool _canEatFood;
        public bool _wantsFood;
        public bool _isFollowingStick;
        public bool _isStealthTargetClose;
        public bool _needToStealth;

        public float _listenDistance;
        public float _petDistance;

        public DogStateMachineInput(PlayerState playerState, int connectionStage,
            float playerDogDistance, bool dogReachedTarget, bool dogFollowingTarget, 
            bool dogFollowingTOI, bool dogBusy,
            float listenDistance, bool isFoodClose, bool canEatFood,
            bool wantsFood, float petDistance, bool isFollowingStick, 
            bool isStealthTargetClose, bool needToStealth)
        {
            _playerState = playerState;
            _playerDogDistance = playerDogDistance;
            _dogReachedTarget = dogReachedTarget;
            _dogFollowingTarget = dogFollowingTarget;
            _dogFollowingTOI = dogFollowingTOI;
            _dogBusy = dogBusy;
            _listenDistance = listenDistance;
            _isFoodClose = isFoodClose;
            _connectionStage = connectionStage;
            _petDistance = petDistance;
            _isFollowingStick = isFollowingStick;
            _canEatFood = canEatFood;
            _wantsFood = wantsFood;
            _isStealthTargetClose = isStealthTargetClose;
            _needToStealth = needToStealth;
        }
    }
}