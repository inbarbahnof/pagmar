using System;
using System.Collections;
using System.Runtime.InteropServices;
using Interactables;
using Targets;
using TMPro;
using UnityEngine;

namespace Dog
{
    public class DogActionManager : MonoBehaviour
    {
        private DogState curState = DogState.Idle;

        [SerializeField] private PlayerStateManager playerStateManager;
        [SerializeField] private TargetGenerator _targetGenerator;
        
        [Header("Distances For States")]
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
        private bool _isFollowingStick;
        private bool _canEatFood;
        private bool _wantFood;
        private bool _isDogProtected;
        private bool _isStealthTargetClose;
        private bool _needToStealth;
        private bool _followingCall;
        private bool _isThereGhostie;
        private int _numberChaseGhostie;

        private DogStateComputer _computer;
        private float _dogPlayerDistance;
        private DogAnimationManager _animationManager;

        private Coroutine _stopFollowCoroutine;
        private Coroutine _chaseGhostieCoroutine;
        
        private bool _movementEnabled = true;
        
        // getters
        public float DogPlayerDistance => _dogPlayerDistance;
        public float ListenDistance => _listenDistance;
        public bool IsDogProtected => _isDogProtected;
        public DogState CurState => curState;
        public bool IsRunning => _playerFollower.IsRunning;

        private void Start()
        {
            _computer = new DogStateComputer();

            _playerFollower = GetComponent<PlayerFollower>();

            _playerFollower.OnIdleAfterTarget += HandleDogIdle;
            _playerFollower.OnStartFollowTOI += HandleDogFollowTOI;
            _playerFollower.OnStartFollow += HandleDogFollow;
            _playerFollower.OnPerformingTargetAction += HandleDogOnAction;
            _playerFollower.OnFinishedTargetAction += HandleDogFinishedAction;
            
            PathTarget.OnDogProtectionChanged += HandleDogProtectionChanged;

            _playerTransform = playerStateManager.GetComponent<Transform>();

            _animationManager = GetComponent<DogAnimationManager>();

            if (_movementEnabled) StartWalkingAfterPlayer();
        }

        private void Update()
        {
            // print("dog State " + curState + " player state " + playerStateManager.CurrentState);
            // print("_numberChaseGhostie " + _numberChaseGhostie);
            // print("following call " + _followingCall);
            if (!_movementEnabled) return;
            
            _dogPlayerDistance = Vector2.Distance(_playerTransform.position, transform.position);
            
            _canEatFood = _targetGenerator.GetFoodTarget() != null;
            _isStealthTargetClose = _targetGenerator.GetStealthTarget(false) != null;

            _isThereGhostie = _targetGenerator.IsThereGhositeClose();
            
            DogStateMachineInput newInput = new DogStateMachineInput(playerStateManager.CurrentState, 
                GameManager.instance.ConnectionState,
                _dogPlayerDistance, _dogReachedTarget,
                _dogFollowingTarget, _dogFollowingTOI,
                _dogBusy, _listenDistance, _foodIsClose, _canEatFood, 
                _wantFood, _petDistance, _isFollowingStick, 
                _isStealthTargetClose, _needToStealth, 
                _followingCall, _isThereGhostie, _numberChaseGhostie);
            
            // print("can eat food " + _canEatFood +" is food close " + _foodIsClose);
            
            DogState newState = _computer.Compute(curState, newInput);
            // print("_dogPlayerDistance " + _dogPlayerDistance);

            if (curState != newState )  
            {
                // print("cur state " + curState + " newState " + newState + " _dogReachedTarget " +_dogReachedTarget);
                HandleDogStateChange(newState);
            }
            else if (curState == DogState.Stealth && _targetGenerator.DidStealthTargetChange())
            {
                HandleStealthBehavior();
            }
        }
        
        public void SetMovementEnabled(bool enabled)
        {
            _movementEnabled = enabled;

            if (!enabled)
            {
                _playerFollower.StopGoingToTarget();
                _playerFollower.SetIsGoingToTarget(false);
                _playerFollower.SetSpeed(0f, true); // Optional: stop animations immediately
            }
            else
            {
                StartWalkingAfterPlayer();
            }
        }

        public void Bark()
        {
            _animationManager.DogBark();
        }

        public void Growl()
        {
            _animationManager.DogGrowl();
        }

        public void SetWantsFood(bool want)
        {
            //print("in SetWantsFood");
            _wantFood = want;
        }

        public void Running(bool isRunning)
        {
            if (isRunning)
                _playerFollower.SetSpeed(7.5f,false);
            else
                _playerFollower.SetSpeed(7.5f,true);
        }

        public void ResetToCheckpoint(Vector2 position)
        {
            _playerFollower.ResetToCheckpoint(position);
            curState = DogState.Idle;
            _numberChaseGhostie = 0;
            
            if (_chaseGhostieCoroutine != null) StopCoroutine(_chaseGhostieCoroutine);
            
            StartCoroutine(DogResetPause());
        }

        private IEnumerator DogResetPause()
        {
            yield return new WaitForSeconds(1f);
            if (curState == DogState.Idle) StartWalkingAfterPlayer();
        }

        private void HandleDogStateChange(DogState newState)
        {
            if (!_movementEnabled) return;
            
            // print("swap from " + curState + " to " + newState);
            if (curState == DogState.Push)
                _playerFollower.SetStopProb(false);
            
            switch (curState, newState)
            {
                case (_, DogState.Stealth):
                    HandleStealthBehavior();
                    break;
                case (_, DogState.WantFood):
                    HandleWantFoodBehavior();
                    break;
                case (_, DogState.FollowFood):
                    curState = DogState.FollowFood;
                    HandleDogFollowTOI();
                    _playerFollower.GoToFoodTarget(_targetGenerator.GetFoodTarget());
                    break;
                case (_, DogState.ChaseGhostie):
                    curState = DogState.ChaseGhostie;
                    _numberChaseGhostie++;
                    if (_chaseGhostieCoroutine == null) 
                        _chaseGhostieCoroutine = StartCoroutine(ZeroChageGhostie());
                    _playerFollower.GoToFoodTarget(_targetGenerator.GetClosestGhostie(transform));
                    Running(true);
                    Bark();
                    break;
                case (_, DogState.FollowCall):
                    curState = DogState.FollowCall;
                    _followingCall = true;
                    if (_stopFollowCoroutine != null) StopCoroutine(_stopFollowCoroutine);
                    _stopFollowCoroutine = StartCoroutine(StopFollowCall());
                    _playerFollower.GoToCallTarget(_targetGenerator.GetCallTarget());
                    break;
                case (_, DogState.FollowStick):
                    HandleFollowStickBehavior();
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
                    Running(false);
                    break;
                case (_, DogState.Push):
                    curState = DogState.Push;
                    _playerFollower.SetStopProb(true);
                    _playerFollower.SetIsGoingToTarget(false);
                    break;
                case (DogState.FollowTOI, DogState.Follow):
                    Target newTarget = _targetGenerator.GenerateNewTarget();
                    Running(false);
                    _playerFollower.SetNextTarget(newTarget);
                    break;
                case (_, DogState.Follow):
                    Target newTarget1 = _targetGenerator.GenerateNewTarget();
                    curState = DogState.Follow;
                    _dogReachedTarget = false;
                    Running(false);
                    _playerFollower.SetNextTarget(newTarget1);
                    _playerFollower.GoToNextTarget();
                    break;
                case (_, DogState.Idle):
                    HandleIdleBehavior();
                    break;
            }
        }

        private IEnumerator ZeroChageGhostie()
        {
            print("start zero coroutine");
            yield return new WaitForSeconds(15f);
            print("stop zero coroutine");
            _numberChaseGhostie = 0;
        }

        private void HandleStealthBehavior()
        {
            curState = DogState.Stealth;
            Target target = _targetGenerator.GetStealthTarget(true);
            _playerFollower.GoToFoodTarget(target); 
        }

        private void HandleWantFoodBehavior()
        {
            curState = DogState.WantFood;
            WantFoodTarget target = _targetGenerator.GetWantFoodTarget();
            _playerFollower.GoToFoodTarget(target); 
        }

        private void HandleFollowStickBehavior()
        {
            _isFollowingStick = true;
            
            curState = DogState.FollowStick;
            _playerFollower.GoToCallTarget(_targetGenerator.GetStickTarget());
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

        public void StealthObs(bool isStealth)
        {
            _needToStealth = isStealth;

            if (!isStealth)
            {
                _playerFollower.StopGoingToTarget();
                curState = DogState.Idle;
            }
        }
        
        public void HandleDogProtectionChanged(bool isProtected)
        {
            _isDogProtected = isProtected;
        }

        public void FoodIsClose(Collider2D food)
        {
            _foodIsClose = true;
            _targetGenerator.NotifyFoodNearby(food.GetComponent<FoodTarget>());
        }

        public void FoodIsFar(Collider2D food)
        {
            if (_dogFollowingTOI) return;
            
            _foodIsClose = false;
            _targetGenerator.NotifyFoodFar(food.GetComponent<FoodTarget>());
        }

        private void HandleIdleBehavior()
        {
            curState = DogState.Idle;
            _dogFollowingTarget = false;
            _dogFollowingTOI = false;
            _playerFollower.SetIsGoingToTarget(false);
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
            _playerFollower.SetSpeed(1f, true);
            _dogFollowingTOI = false;
            _dogBusy = true;
            _dogReachedTarget = true;
            _isFollowingStick = false;
        }

        private void HandleDogFinishedAction()
        {
            _playerFollower.SetSpeed(1f, true);
            _dogFollowingTOI = false;
            _dogBusy = false;
            _isFollowingStick = false;
            _wantFood = false;
            _needToStealth = false;
        }

        private IEnumerator StopFollowCall()
        {
            yield return new WaitForSeconds(4f);
            _followingCall = false;
        }

        private void HandleDogFollow()
        {
            _dogFollowingTarget = true;
            _dogReachedTarget = false;
        }

        private void HandleDogIdle()
        {
            _playerFollower.SetSpeed(1f, true);
            _dogFollowingTOI = false;
            _dogFollowingTarget = false;
            _dogReachedTarget = true;
        }
    }
}