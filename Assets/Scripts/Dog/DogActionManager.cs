using System;
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
        [SerializeField] private RandomTargetGenerator _targetGenerator;
        [SerializeField] private float _pushDistance = 2f;

        private PlayerFollower _playerFollower;
        private Transform _playerTransform;

        private bool _dogReachedTarget;
        private bool _dogFollowingTarget;
        private bool _dogFollowingTOI;
        private bool _dogBusy;
        private bool _isBeingCalled;

        private DogStateComputer _computer;

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
            
            float curDistance = Vector2.Distance(_playerTransform.position, transform.position);
            DogStateMachineInput newInput = new DogStateMachineInput(playerStateManager.CurrentState, curDistance,
                _dogReachedTarget, _dogFollowingTarget, _dogFollowingTOI, _pushDistance, _dogBusy);
            DogState newState = _computer.Compute(curState, newInput);

            if (curState != newState)
            {
                HandleDogStateChange(newState);
            }
        }

        private void HandleDogStateChange(DogState newState)
        {
            // print("swap from " + curState + " to " + newState);
            switch (curState, newState)
            {
                case (_, DogState.FollowCall):
                    curState = DogState.FollowCall;
                    _playerFollower.GoToCallTarget(_targetGenerator.GetCallTarget());
                    break;
                case (_, DogState.OnTargetAction):
                    curState = DogState.OnTargetAction;
                    break;
                case (_, DogState.Push):
                    curState = DogState.Push;
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

        private void HandleIdleBehavior()
        {
            curState = DogState.Idle;
            _dogFollowingTarget = false;
            _playerFollower.SetIsGoingToTarget(false);
            // change to random idle animation 
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
            _dogBusy = true;
        }

        private void HandleDogFinishedAction()
        {
            _dogBusy = false;
        }

        private void HandleDogFollow()
        {
            _dogFollowingTarget = true;
        }

        private void HandleDogIdle()
        {
            _dogReachedTarget = true;
        }

        // private void HandlePlayerPushBehavior()
        // {
        //     float distance = Vector2.Distance(_playerTransform.position, transform.position);
        //     
        //     if (distance < _pushDistance)
        //     {
        //         curState = DogState.Push;
        //         _playerFollower.SetIsGoingToTarget(false);
        //     }
        // }
        //
        // private void HandlePlayerWalkBehavior()
        // {
        //     // float distanceToPlayer = Vector3.Distance(transform.position, playerStateManager.transform.position);
        //
        //     Target newTarget = _targetGenerator.GenerateNewTarget();
        //     if (newTarget == null) return;
        //     
        //     // check if dog is on idle or action
        //     switch (curState)
        //     {
        //         case DogState.Idle:
        //             curState = DogState.Follow;
        //             _playerFollower.SetNextTarget(newTarget);
        //             _playerFollower.GoToNextTarget();
        //             break;
        //         case DogState.FollowTOI:
        //             _playerFollower.SetNextTarget(newTarget);
        //             break;
        //     }
        //     
        // }
        //
        // private void HandlePlayerIdleBehavior()
        // {
        //     curState = DogState.Idle;
        // }
    }
}