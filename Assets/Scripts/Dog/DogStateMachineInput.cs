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
        public bool _followingCall;
        public bool _isThereGhostie;
        public int _chaseGhostieNumber;
        public bool _chasingGhostie;
        public bool _isPetting;
        public bool _eating;
        public bool _waitForPet;

        public float _listenDistance;
        public float _petDistance;

        public DogStateMachineInput(PlayerState playerState, int connectionStage,
            float playerDogDistance, bool dogReachedTarget, bool dogFollowingTarget, 
            bool dogFollowingTOI, bool dogBusy,
            float listenDistance, bool isFoodClose, bool canEatFood,
            bool wantsFood, float petDistance, bool isFollowingStick, 
            bool isStealthTargetClose, bool needToStealth, 
            bool followingCall, bool isThereGhostie, int chaseGhostieNumber,
            bool chasingGhostie, bool isPetting, bool eating, bool waitForPet)
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
            _followingCall = followingCall;
            _isThereGhostie = isThereGhostie;
            _chaseGhostieNumber = chaseGhostieNumber;
            _chasingGhostie = chasingGhostie;
            _isPetting = isPetting;
            _eating = eating;
            _waitForPet = waitForPet;
        }
    }
}