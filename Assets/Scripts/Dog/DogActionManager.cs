using System;
using System.Collections;
using System.Runtime.InteropServices;
using Interactables;
using Targets;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Dog
{
    public class DogActionManager : MonoBehaviour
    {
        [SerializeField] private bool movementEnabled = true;
        [SerializeField] private PlayerStateManager playerStateManager;
        [SerializeField] private TargetGenerator _targetGenerator;
        [SerializeField] private ParticleSystem _soundParticals;
        [SerializeField] private Animator _shadowAnimator;
        
        [Header("Distances For States")]
        [SerializeField] private float _listenDistance = 6f;
        [SerializeField] private float _petDistance = 2f;

        private PlayerFollower _playerFollower;
        private Transform _playerTransform;
        
        private DogState curState = DogState.Idle;

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
        private bool _crouching;
        private bool _chasingGhostie;
        private bool _eating;
        private bool _jumping;
        private bool _waitForPet;

        private DogStateComputer _computer;
        private float _dogPlayerDistance;
        private DogAnimationManager _animationManager;
        private SmoothMover _mover;

        private Coroutine _stopFollowCoroutine;
        private Coroutine _chaseGhostieCoroutine;
        
        
        // getters
        public float DogPlayerDistance => _dogPlayerDistance;
        public float ListenDistance => _listenDistance;
        public bool IsDogProtected => _isDogProtected;
        public bool Crouching => _crouching;
        public DogState CurState => curState;
        public bool IsRunning => _playerFollower.IsRunning;

        private void Start()
        {
            _computer = new DogStateComputer();

            _playerFollower = GetComponent<PlayerFollower>();
            _mover = GetComponent<SmoothMover>();

            _playerFollower.OnIdleAfterTarget += HandleDogIdle;
            _playerFollower.OnStartFollowTOI += HandleDogFollowTOI;
            _playerFollower.OnStartFollow += HandleDogFollow;
            _playerFollower.OnPerformingTargetAction += HandleDogOnAction;
            _playerFollower.OnFinishedTargetAction += HandleDogFinishedAction;
            
            PathTarget.OnDogProtectionChanged += HandleDogProtectionChanged;

            _playerTransform = playerStateManager.GetComponent<Transform>();

            _animationManager = GetComponent<DogAnimationManager>();
            if (GameManager.instance.Chapter >= 3) _animationManager.SetDogInjured();

            if (movementEnabled) StartWalkingAfterPlayer();
            else SetMovementEnabled(false);
        }

        private void Update()
        {
            // print("dog State " + curState + " player state " + playerStateManager.CurrentState);
            // print("_foodIsClose " + _foodIsClose);
            if (!movementEnabled) return;
            
            _dogPlayerDistance = Vector2.Distance(_playerTransform.position, transform.position);
            
            _canEatFood = _targetGenerator.GetFoodTarget() != null;
            _isStealthTargetClose = _targetGenerator.GetStealthTarget(false) != null;

            _isThereGhostie = _targetGenerator.IsThereGhositeClose();

            bool eating = _animationManager.Eating;
            DogStateMachineInput newInput = new DogStateMachineInput(playerStateManager.CurrentState, 
                GameManager.instance.ConnectionState,
                _dogPlayerDistance, _dogReachedTarget,
                _dogFollowingTarget, _dogFollowingTOI,
                _dogBusy, _listenDistance, _foodIsClose, _canEatFood, 
                _wantFood, _petDistance, _isFollowingStick, 
                _isStealthTargetClose, _needToStealth, 
                _followingCall, _isThereGhostie, _numberChaseGhostie, 
                _chasingGhostie, _animationManager.Petting, eating, _waitForPet);
            
            DogState newState = _computer.Compute(curState, newInput);

            if (curState != newState)  
            {
                // print("cur state " + curState + " newState " + newState + " _dogReachedTarget " +_dogReachedTarget);
                HandleDogStateChange(newState);
            }
            else if (curState == DogState.Stealth && _targetGenerator.DidStealthTargetChange())
            {
                HandleStealthBehavior();
            }
        }

        public void DogBarkGrowl()
        {
            _playerFollower.ResetDogPos(transform.position);
            _animationManager.BarkGrowlAnimations(_playerTransform);
        }
        
        public void SetMovementEnabled(bool enabled)
        {
            movementEnabled = enabled;

            if (!enabled)
            {
                _playerFollower.StopGoingToTarget();
                _playerFollower.SetIsGoingToTarget(false);
                _playerFollower.SetSpeed(true);
                _animationManager.SetAnimationEnabled(false);
                // _animationManager.
            }
            else
            {
                _animationManager.SetAnimationEnabled(true);

                var agent = GetComponent<NavMeshAgent>();
                if (agent != null) agent.Warp(transform.position);
                if (GameManager.instance.Chapter > 0) StartWalkingAfterPlayer();
            }
        }

        public void FollowPlayer()
        {
            StartWalkingAfterPlayer();
        }

        public void Bark()
        {
            _animationManager.DogBark();
            if (_needToStealth) _soundParticals.Play();
        }

        public void DogJump(bool fromLeft)
        {
            if (_jumping) return;
            
            _jumping = true;
            _animationManager.DogJumping(true);
            StartCoroutine(JumpMovement(fromLeft));
        }

        private IEnumerator JumpMovement(bool fromLeft)
        {
            _playerFollower.PauseAgentMovement();
            yield return new WaitForSeconds(0.12f);
            _shadowAnimator.SetTrigger("out");
            
            Vector3 startPos = transform.position;
            float times = fromLeft ? 1f : -1f;
            Vector3 targetPos = startPos + new Vector3(3f * times, 0f, 0f); // jump right; use -1.5f for left
            
            float jumpDuration = 0.56f;
            float elapsed = 0f;

            while (elapsed < jumpDuration)
            {
                float t = elapsed / jumpDuration;
                transform.position = Vector3.Lerp(startPos, targetPos, t);
                elapsed += Time.deltaTime;
                if (t >= 0.7) _shadowAnimator.SetTrigger("in");
                yield return null;
            }

            transform.position = targetPos;
            yield return new WaitForSeconds(0.12f);
            _playerFollower.ResumeAgentMovement();
            _jumping = false;
        }

        public void WaitUntilEating()
        {
            StartCoroutine(WantFoodBehavior());
        }

        private IEnumerator WantFoodBehavior()
        {
            _animationManager.DogBark();
            yield return new WaitForSeconds(0.4f);
            
            _animationManager.DogWaitForFood();
            yield return new WaitForSeconds(2f);
            
            while (curState == DogState.WantFood)
            {
                int choice = Random.Range(0, 3); // 0: Bark, 1: HappyCrouch, 2: Dig

                switch (choice)
                {
                    case 0:
                        _animationManager.DogBark();
                        yield return new WaitForSeconds(0.4f);
                        break;
                    case 1:
                        _animationManager.DogWaitForFood();
                        yield return new WaitForSeconds(2f);
                        break;
                    case 2:
                        _animationManager.DogDig();
                        yield return new WaitForSeconds(2f);
                        break;
                }
            }
        }

        public void Growl(Transform growlAt, bool atPlayer)
        {
            if (!atPlayer) _animationManager.DogGrowl(growlAt, atPlayer);
            else _playerFollower.GoToFoodTarget(_targetGenerator.GetPetTarget());
        }

        public void SetWantsFood(bool want)
        {
            // print("in SetWantsFood");
            _wantFood = want;
        }

        public void Running(bool isRunning)
        {
            if (isRunning)
                _playerFollower.SetSpeed(false);
            else
                _playerFollower.SetSpeed(true);
        }

        public void DogStop()
        {
            _playerFollower.StopGoingToTarget();
        }

        public void ResetToCheckpoint(Vector2 position)
        {
            _playerFollower.ResetToCheckpoint(position);
            curState = DogState.Idle;
            _numberChaseGhostie = 0;
            _wantFood = false;
            _dogFollowingTarget = false;
            _dogBusy = false;
            _dogReachedTarget = true;
            _dogFollowingTOI = false;
            
            if (_chaseGhostieCoroutine != null) StopCoroutine(_chaseGhostieCoroutine);
            
            StartCoroutine(DogResetPause());
        }

        private void HandleWaitForPetBehavior()
        {
            _playerFollower.StopGoingToTarget();
            _playerFollower.SetIsGoingToTarget(false);
            
            _animationManager.DogWaitForPet();
        }

        public void StopWaitingForPet()
        {
            _waitForPet = false;
        }

        public void WaitForPetBehavior()
        {
            _waitForPet = true;
        }

        private IEnumerator DogResetPause()
        {
            yield return new WaitForSeconds(1f);
            if (curState == DogState.Idle) StartWalkingAfterPlayer();
        }

        private void HandleDogStateChange(DogState newState)
        {
            if (!movementEnabled) return;
            
            // print("swap from " + curState + " to " + newState);
            if (curState == DogState.Push)
                _playerFollower.SetStopProb(false);
            
            switch (curState, newState)
            {
                case (_, DogState.Stealth):
                    HandleStealthBehavior();
                    break;
                case (_, DogState.WaitForPet):
                    HandleWaitForPetBehavior();
                    break;
                case (_, DogState.WantFood):
                    StartCoroutine(HandleWantFoodBehavior());
                    break;
                case (_, DogState.FollowFood):
                    curState = DogState.FollowFood;
                    HandleDogFollowTOI();
                    FoodTarget food = _targetGenerator.GetFoodTarget();
                    _playerFollower.GoToFoodTarget(food);
                    food.SetCantPickUp();
                    break;
                case (_, DogState.ChaseGhostie):
                    HandleChaseGhostie();
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
                case (_, DogState.Growl):
                    curState = DogState.Growl;
                    Growl(_playerTransform, true);
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

        private void HandleChaseGhostie()
        {
            curState = DogState.ChaseGhostie;
            _numberChaseGhostie++;
            
            if (_chaseGhostieCoroutine != null)
                StopCoroutine(_chaseGhostieCoroutine);
            _chaseGhostieCoroutine = StartCoroutine(ZeroChaseGhostie());
            
            _playerFollower.GoToFoodTarget(_targetGenerator.GetClosestGhostie(transform));
            Running(true);
            Bark();
            _chasingGhostie = true;
        }

        private IEnumerator ZeroChaseGhostie()
        {
            yield return new WaitForSeconds(11f);
            _numberChaseGhostie = 0;
        }

        private void HandleStealthBehavior()
        {
            curState = DogState.Stealth;
            Target target = _targetGenerator.GetStealthTarget(true);
            _playerFollower.GoToFoodTarget(target); 
        }

        private IEnumerator HandleWantFoodBehavior()
        {
            _playerFollower.StopGoingToTarget();
            _playerFollower.SetIsGoingToTarget(false);
            
            if (_foodIsClose)
            {
                _animationManager.DogAirSniff();
                yield return new WaitForSeconds(1.5f);
            }
            
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
            _animationManager.PlayerPet();
            _playerFollower.GoToFoodTarget(_targetGenerator.GetPetTarget());
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

        public void ChangeCrouching(bool crouch)
        {
            _crouching = crouch;
            _playerFollower.SetStealth(crouch);
        }

        public void FoodIsClose(Collider2D food)
        {
            _foodIsClose = true;
            FoodTargetGetter getter = food.GetComponent<FoodTargetGetter>();
            _targetGenerator.NotifyFoodNearby(getter.GetFoodTarget());
        }

        public void FoodNotClose()
        {
            _foodIsClose = false;
        }

        public void FoodIsFar(Collider2D food)
        {
            if (_dogFollowingTOI) return;
            _foodIsClose = false;
            _targetGenerator.NotifyFoodFar(food.GetComponent<FoodTarget>());
        }

        private void HandleIdleBehavior()
        {
            IdleBehavior();
            StartCoroutine(IdleDecisionAfterDelay());
        }

        private bool GoToTOI()
        {
            //print("GoToTOI");
            Target target = _targetGenerator.GetNearbyTOI(transform);
            if (target != null)
            {
                _dogFollowingTOI = true;
                curState = DogState.FollowTOI;
                _playerFollower.GoToTOI(target);
                return true;
            }

            return false;
        }

        private void IdleBehavior()
        {
            curState = DogState.Idle;
            _dogFollowingTarget = false;
            _dogFollowingTOI = false;
            _playerFollower.SetIsGoingToTarget(false);
        }
        
        private IEnumerator IdleDecisionAfterDelay()
        {
            while (curState == DogState.Idle)
            {
                yield return new WaitForSeconds(5f);
                
                // If the state has changed during the wait, abort
                if (curState != DogState.Idle)
                    yield break;

                if (!_playerFollower.IsRunning && Random.value < 0.4f)
                {
                    bool didGoTOI = GoToTOI();
                    if (didGoTOI) yield break;
                }

                IdleBehavior();
            }
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
            if (GameManager.instance.Chapter < 5) _playerFollower.SetSpeed(true);
            _dogFollowingTOI = false;
            _dogBusy = true;
            _dogReachedTarget = true;
            _isFollowingStick = false;
        }

        private void HandleDogFinishedAction()
        {
            if (GameManager.instance.Chapter < 5) _playerFollower.SetSpeed(true);
            _dogFollowingTOI = false;
            _dogBusy = false;
            _isFollowingStick = false;
            _chasingGhostie = false;
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
            if (GameManager.instance.Chapter < 5) _playerFollower.SetSpeed(true);
            _dogFollowingTOI = false;
            _dogFollowingTarget = false;
            _dogReachedTarget = true;
        }
    }
}