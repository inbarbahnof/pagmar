using System;
using Interactables;
using Targets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Dog
{
    public class DogActionManager : MonoBehaviour
    {
        private DogState curState = DogState.Idle;
        public DogState CurState => curState;

        [SerializeField] private PlayerStateManager playerStateManager;
        [SerializeField] private TargetGenerator _targetGenerator;
        
        [Header("Distances For States")]
        [SerializeField] private float _pushDistance = 2f;
        [SerializeField] private float _listenDistance = 6f;
        [SerializeField] private float _petDistance = 2f;
        [SerializeField] private float getAwayFromPlayerDis = 3f;

        private PlayerFollower _playerFollower;
        private Transform _playerTransform;

        private bool _dogReachedTarget;
        private bool _dogFollowingTarget;
        private bool _dogFollowingTOI;
        private bool _dogBusy;
        private bool _isBeingCalled;
        private bool _foodIsClose;

        private DogStateComputer _computer;
        private float _dogPlayerDistance;
        public float DogPlayerDistance => _dogPlayerDistance;
        public float ListenDistance => _listenDistance;

        private void Start()
        {
            _computer = new DogStateComputer();

            _playerFollower = GetComponent<PlayerFollower>();

            _playerFollower.OnIdleAfterTarget += HandleDogIdle;
            _playerFollower.OnStartFollowTOI += HandleDogFollowTOI;
            _playerFollower.OnStartFollow += HandleDogFollow;
            _playerFollower.OnPerformingTargetAction += HandleDogOnAction;
            _playerFollower.OnFinishedTargetAction += HandleDogFinishedAction;

            _playerTransform = playerStateManager.GetComponent<Transform>();

            StartWalkingAfterPlayer();
        }

        private void Update()
        {
            // print("dog State " + curState + " player state " + playerStateManager.CurrentState);
            
            _dogPlayerDistance = Vector2.Distance(_playerTransform.position, transform.position);
            DogStateMachineInput newInput = new DogStateMachineInput(playerStateManager.CurrentState, 
                GameManager.instance.ConnectionState,
                _dogPlayerDistance, _dogReachedTarget,
                _dogFollowingTarget, _dogFollowingTOI,
                _pushDistance, _dogBusy, _listenDistance, _foodIsClose, _petDistance);
            
            DogState newState = _computer.Compute(curState, newInput);
            // print("_dogPlayerDistance " + _dogPlayerDistance);

            if (curState != newState || !_dogReachedTarget)
            {
                HandleDogStateChange(newState);
            }
        }

        public void ResetToCheckpoint(Vector2 position)
        {
            _playerFollower.ResetToCheckpoint(position);
            curState = DogState.Idle;

            StartWalkingAfterPlayer();
        }

        private void HandleDogStateChange(DogState newState)
        {
            // print("swap from " + curState + " to " + newState);
            if (curState == DogState.Push)
                _playerFollower.SetStopProb(false);
            
            switch (curState, newState)
            {
                case (_, DogState.FollowFood):
                    curState = DogState.FollowFood;
                    _playerFollower.GoToCallTarget(_targetGenerator.GetFoodTarget());
                    break;
                case (_, DogState.FollowCall):
                    curState = DogState.FollowCall;
                    _playerFollower.GoToCallTarget(_targetGenerator.GetCallTarget());
                    break;
                case (_, DogState.GetAwayFromPlayer):
                    curState = DogState.GetAwayFromPlayer;
                    GetAwayFromPlayer();
                    break;
                case (_, DogState.Pet):
                    curState = DogState.Pet;
                    HandlePetBehavior();
                    break;
                case (DogState.FollowCall, DogState.Follow):
                    curState = DogState.Follow;
                    break;
                case (_, DogState.OnTargetAction):
                    curState = DogState.OnTargetAction;
                    break;
                case (_,DogState.FollowTOI):
                    curState = DogState.FollowTOI;
                    break;
                case (_, DogState.Push):
                    curState = DogState.Push;
                    _playerFollower.SetStopProb(true);
                    _playerFollower.SetIsGoingToTarget(false);
                    break;
                case (DogState.FollowTOI, DogState.Follow):
                    Target newTarget = _targetGenerator.GenerateNewTarget();
                    _playerFollower.SetNextTarget(newTarget);
                    break;
                case (_, DogState.Follow):
                    Target newTarget1 = _targetGenerator.GenerateNewTarget();
                    curState = DogState.Follow;
                    _dogReachedTarget = false;
                    _playerFollower.SetNextTarget(newTarget1);
                    _playerFollower.GoToNextTarget();
                    break;
                case (_, DogState.Idle):
                    HandleIdleBehavior();
                    break;
            }
        }

        private void HandlePetBehavior()
        {
            
        }

        private void GetAwayFromPlayer()
        {
            Vector2 directionAway = (transform.position - _playerTransform.position).normalized;
            Vector2 newPos = (Vector2)transform.position + directionAway * getAwayFromPlayerDis;
            transform.position = newPos;
            
            curState = DogState.Idle;
        }

        public void FoodIsClose(Collider2D food)
        {
            PickUpInteractable pickUp = food.GetComponent<PickUpInteractable>();

            if (pickUp != null && !pickUp.IsPickedUp)
            {
                _foodIsClose = true;
                _targetGenerator.SetFoodTarget(food.GetComponent<Target>());
            }
        }

        private void HandleIdleBehavior()
        {
            curState = DogState.Idle;
            _dogFollowingTarget = false;
            _dogFollowingTOI = false;
            _playerFollower.SetIsGoingToTarget(false);
            // change to random idle animation 

            // Target target = _targetGenerator.GenerateNewIdleTarget();
            // if (target != null)
            // {
            //     _playerFollower.SetNextTarget(target);
            //     _playerFollower.GoToNextTarget();
            // }
        }

        private void StartWalkingAfterPlayer()
        {
            Target newTarget = _targetGenerator.GenerateNewTarget();

            if (newTarget == null)
            {
                print("newTarget == null");
                return;
            }

            curState = DogState.Follow;
            _playerFollower.SetNextTarget(newTarget);
            _playerFollower.GoToNextTarget();
        }

        private void HandleDogFollowTOI()
        {
            _dogFollowingTOI = true;
        }

        private void HandleDogOnAction()
        {
            _dogFollowingTOI = false;
            _dogBusy = true;
            _foodIsClose = false;
        }

        private void HandleDogFinishedAction()
        {
            _dogFollowingTOI = false;
            _dogBusy = false;
            _foodIsClose = false;
        }

        private void HandleDogFollow()
        {
            _dogFollowingTOI = false;
            _dogFollowingTarget = true;
            _dogReachedTarget = false;
        }

        private void HandleDogIdle()
        {
            _dogFollowingTOI = false;
            _dogFollowingTarget = false;
            _dogReachedTarget = true;
        }
    }
}