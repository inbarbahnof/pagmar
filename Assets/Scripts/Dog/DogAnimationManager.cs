using System;
using System.Collections;
using System.Collections.Generic;
using Audio.FMOD;
using FMOD.Studio;
using Spine;
using Spine.Unity;
using Targets;
// using UnityEditor.Rendering;
using UnityEngine;
using Event = UnityEngine.Event;
using Random = UnityEngine.Random;

namespace Dog
{
    public class DogAnimationManager : MonoBehaviour
    {
        [SerializeField] SkeletonAnimation skeletonAnimation;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private ParticleSystem _digParticals;
        [SerializeField] private string _injuredSkinName;

        [Header("Animation Times")]
        [SerializeField] private float _barkAnimationTime = 0.4f;
        [SerializeField] private float _growlAnimationTime = 2f;
        [SerializeField] private float _eatAnimationTime = 0.733f;
        
        [Header("Animation Names")] 
        [SerializeField] private string idleAnimName;
        [SerializeField] private string walkAnimName;
        [SerializeField] private string runAnimName;
        [SerializeField] private string eatAnimName;
        [SerializeField] private string growlAnimName;
        [SerializeField] private string happyCrouchAnimName;
        [SerializeField] private string idleCrouchAnimName;
        [SerializeField] private string jumpAnimName;
        [SerializeField] private string listenAnimName;
        [SerializeField] private string sniffAnimName;
        [SerializeField] private string walkCrouchAnimName;
        [SerializeField] private string barkAnimName;
        [SerializeField] private string digAnimName;
        [SerializeField] private string petAnimName;
        [SerializeField] private string afterEatAnimName;
        [SerializeField] private string airSniffAnimName;
        [SerializeField] private string tailWagAnimName;

        [Header("Animation Speeds")] 
        [SerializeField] private float idleAnimSpeed = 1f;
        [SerializeField] private float walkAnimSpeed = 1f;
        [SerializeField] private float runAnimSpeed = 1.5f;
        [SerializeField] private float eatAnimSpeed = 0.8f;
        [SerializeField] private float growlAnimSpeed = 1f;
        [SerializeField] private float happyCrouchAnimSpeed = 1f;
        [SerializeField] private float idleCrouchAnimSpeed = 1f;
        [SerializeField] private float jumpAnimSpeed = 1f;
        [SerializeField] private float listenAnimSpeed = 1f;
        [SerializeField] private float barkAnimSpeed = 1f;
        [SerializeField] private float sniffAnimSpeed = 0.8f;
        [SerializeField] private float walkCrouchAnimSpeed = 0.8f;
        [SerializeField] private float digAnimSpeed = 1f;
        [SerializeField] private float petAnimSpeed = 1f;
        [SerializeField] private float afterEatAnimSpeed = 1f;
        [SerializeField] private float airSniffAnimSpeed = 1f;
        [SerializeField] private float tailWagAnimSpeed = 1f;
        
        [Header("Event Names")] 
        [SerializeField] private string _footstepsEventName;
        [SerializeField] private string _barkEventName;
        [SerializeField] private string _digEventName;
        [SerializeField] private string _eatEventName;
        [SerializeField] private string _growlEventName;
        [SerializeField] private string _happyCrouchEventName;
        [SerializeField] private string _sniffEventName;
    
        private Spine.EventData _footstepsEventData;
        private Spine.EventData _barkEventData;
        private Spine.EventData _digEventData;
        private Spine.EventData _eatEventData;
        private Spine.EventData _growlEventData;
        private Spine.EventData _happyCrouchEventData;
        private Spine.EventData _sniffEventData;
        
        private DogAnimation _curAnim;
        
        private bool _isMoving;
        private bool _growling;
        private bool _eating;
        private bool _afterEat;
        private bool _sniffing;
        private bool _listening;
        private bool _wantFood;
        private bool _isJumping;
        private bool _petting;
        private bool _digging;
        private bool _airSniff;
        private bool _tailWag;
        private bool _idleWagging;
        private bool _smoothMoving;

        public bool Petting => _petting;
        public bool Eating => _afterEat || _eating;
        
        private Spine.AnimationState spineAnimationState;
        
        [SerializeField] private GameObject art;

        private DogActionManager _actionManager;
        private Vector3 lastPosition;
        private float moveXPrevDir;
        
        private DogAnimation _currentIdleBehavior = DogAnimation.Idle;
        private float _nextIdleBehaviorChangeTime = 0f;
        private float _idleBehaviorDuration = 2.5f;
        private bool _animationEnabled = true;
        
        public void SetAnimationEnabled(bool enabled)
        {
            _animationEnabled = enabled;
            
            if (!enabled)
            {
                spineAnimationState ??= skeletonAnimation.AnimationState;
                // spineAnimationState.ClearTracks();
            }
        }
        
        public void SetDogInjured()
        {
            print("dog injured ");
            var skeleton = skeletonAnimation.Skeleton;
            
            skeleton.SetSkin(_injuredSkinName);
            skeleton.SetSlotsToSetupPose();
            skeletonAnimation.AnimationState.Apply(skeleton);
        }
        
        private void Start()
        {
            _actionManager = GetComponent<DogActionManager>();
            lastPosition = transform.position;
            if (spineAnimationState is null) spineAnimationState = skeletonAnimation.AnimationState;
            
            _footstepsEventData = skeletonAnimation.Skeleton.Data.FindEvent(_footstepsEventName);
            _barkEventData = skeletonAnimation.Skeleton.Data.FindEvent(_barkEventName);
            _digEventData = skeletonAnimation.Skeleton.Data.FindEvent(_digEventName);
            _eatEventData = skeletonAnimation.Skeleton.Data.FindEvent(_eatEventName);
            _growlEventData = skeletonAnimation.Skeleton.Data.FindEvent(_growlEventName);
            _happyCrouchEventData = skeletonAnimation.Skeleton.Data.FindEvent(_happyCrouchEventName);
            _sniffEventData = skeletonAnimation.Skeleton.Data.FindEvent(_sniffEventName);
            
            skeletonAnimation.AnimationState.Event += HandleAnimationStateEvent;
            
            DogAnimationUpdate(DogAnimation.Idle);
        }
        
        private void HandleAnimationStateEvent (TrackEntry trackEntry, Spine.Event e)
        {
            if (e.Data == _footstepsEventData) 
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.DogFootsteps, 
                    transform.position, true);
            else if(e.Data == _barkEventData)
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.DogBark, 
                    transform.position, true);
            else if(e.Data == _eatEventData)
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.DogEat, 
                    transform.position, true);
            else if(e.Data == _growlEventData)
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.DogGrowl, 
                    transform.position, true);
            else if(e.Data == _sniffEventData)
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.DogSniff, 
                    transform.position, true);
            else if(e.Data == _digEventData)
                _digParticals.Play();
            
            // else if(e.Data == _happyCrouchEventData)
        }

        private void Update()
        {
            if (!_animationEnabled) return;
            
            if (!_petting && !_growling && !_eating && !_afterEat && !_smoothMoving) CheckRotation();
            UpdateMoving();
            DogAnimation cur = WhichAnimShouldBePlayed();
            if (cur != _curAnim) DogAnimationUpdate(cur);
        }

        private void CheckRotation()
        {
            Vector3 currentPosition = transform.position;
            float dirX = currentPosition.x - lastPosition.x;
            
            if (Mathf.Abs(dirX) > 0.001f && _actionManager.CurState != DogState.Push)
            {
                float newScaleX = dirX > 0 ? 1 : -1;
                art.transform.localScale = new Vector3(
                    newScaleX * Mathf.Abs(art.transform.localScale.x),
                    art.transform.localScale.y,
                    art.transform.localScale.z
                );
            }
        }

        private void UpdateMoving()
        {
            Vector3 currentPosition = transform.position;
            float distanceMoved = Vector3.Distance(currentPosition, lastPosition);
            
            if (_actionManager.Crouching) _isMoving = distanceMoved > 0.005f;
            else _isMoving = distanceMoved > 0.005f;
            
            lastPosition = currentPosition; 
        }

        private DogAnimation WhichAnimShouldBePlayed()
        {
            if (_eating) return DogAnimation.Eat;
            if (_afterEat) return DogAnimation.AfterEat;
            if (_petting) return DogAnimation.Pet;
            if (_isJumping) return DogAnimation.Jump;
            if (_tailWag) return DogAnimation.TailWag;
            if (_smoothMoving) return DogAnimation.Walk;
            if (_listening) return DogAnimation.Listen;
            
            if (_actionManager.Crouching)
            {
                if (_isMoving) return DogAnimation.WalkCrouch;
                return DogAnimation.IdleCrouch;
            }
            
            if (_isMoving)
            {
                if (_actionManager.IsRunning) return DogAnimation.Run;
                return DogAnimation.Walk;
            }

            if (_airSniff) return DogAnimation.AirSniff;
            if (_wantFood) return DogAnimation.HappyCrouch;
            if (_sniffing) return DogAnimation.Sniff;
            if (_digging) return DogAnimation.Dig;
            if (_growling) return DogAnimation.Growl;
            if (_idleWagging) return DogAnimation.TailWag;
            
            if (Time.time >= _nextIdleBehaviorChangeTime)
            {
                _currentIdleBehavior = PickRandomIdleBehavior();
                _nextIdleBehaviorChangeTime = Time.time + _idleBehaviorDuration;
            }

            return _currentIdleBehavior;
        }
        
        private DogAnimation PickRandomIdleBehavior()
        {
            DogAnimation[] idleOptions = {
                DogAnimation.Idle,
                DogAnimation.Listen,
                DogAnimation.Dig
            };

            int index = Random.Range(0, idleOptions.Length);
            return idleOptions[index];
        }

        public void IdleTailWag(bool isWagging)
        {
            _idleWagging = isWagging;
        }
        
        public void ListenAndGrowl()
        {
            StartCoroutine(ListenAndGrowlCoroutine());
        }

        private IEnumerator ListenAndGrowlCoroutine()
        {
            _listening = true;
            yield return new WaitForSeconds(0.8f);
            _listening = false;

            _growling = true;
            yield return new WaitForSeconds(1f);
            _growling = false;
        }
        
        public void UpdateSmoothMove(bool move, Vector2 faceTo)
        {
            _smoothMoving = move;
            if (move) FaceTowardsPosition(faceTo);
        }

        public void PetBehavior()
        {
            FaceTowardsTransform(playerTransform);
            _petting = true;
            StartCoroutine(PetCorutine());
        }

        private IEnumerator PetCorutine()
        {
            yield return new WaitForSeconds(2f);
            _petting = false;

            _tailWag = true;
            yield return new WaitForSeconds(2f);
            _tailWag = false;
        }

        public void DogDig()
        {
            _digging = true;
            StartCoroutine(StopSniffing());
        }

        public void DogStartSniff()
        {
            _sniffing = true;
            StartCoroutine(StopSniffing());
        }

        private IEnumerator StopSniffing()
        {
            yield return new WaitForSeconds(2f);
            _sniffing = false;
            _digging = false;
        }
        
        public void DogFinishSniff()
        {
            _sniffing = false;
        }

        public void DogJumping(bool jump)
        {
            _isJumping = jump;
            StartCoroutine(FinishJump());
        }
        
        private IEnumerator FinishJump()
        {
            yield return new WaitForSeconds(0.8f);
            _isJumping = false;
        }

        public void DogAirSniff()
        {
            _airSniff = true;
            StartCoroutine(FinishAirSniff());
        }
        
        private IEnumerator FinishAirSniff()
        {
            yield return new WaitForSeconds(1.5f);
            _airSniff = false;
        }

        public void DogWaitForFood()
        {
            _wantFood = true;
            StartCoroutine(WaitToAnimation());
        }

        private IEnumerator WaitToAnimation()
        {
            yield return new WaitForSeconds(2f);
            _wantFood = false;
        }

        public void DogBark()
        {
            StartCoroutine(Bark());
        }
        
        private IEnumerator Bark()
        {
            TrackEntry entry = null;
            entry = spineAnimationState.SetAnimation(1, barkAnimName, false);
            if (entry != null) entry.TimeScale = barkAnimSpeed;
            
            yield return new WaitForSeconds(_barkAnimationTime);
            
            spineAnimationState.ClearTrack(1);
        }

        public void DogGrowl(Transform growlAt, bool atPlayer)
        {
            if (atPlayer) FaceTowardsTransform(playerTransform);
            else FaceTowardsTransform(growlAt);
            StartCoroutine(Growl());
        }

        public void DogEat(Transform foodTransform, Action DestroyFood, Action onDoneEating)
        {
            FaceTowardsTransform(foodTransform);
            StartCoroutine(Eat(DestroyFood, onDoneEating));
        }

        private IEnumerator Eat(Action DestroyFood, Action onDoneEating)
        {
            _eating = true;
            yield return new WaitForSeconds(_eatAnimationTime);
            
            DestroyFood?.Invoke();
            _eating = false;
            
            _afterEat = true;
            yield return new WaitForSeconds(1.3f);
            _afterEat = false;
            
            onDoneEating?.Invoke();
        }
        
        private IEnumerator Growl()
        {
            _sniffing = false;
            _growling = true;
            
            yield return new WaitForSeconds(_growlAnimationTime);

            _growling = false;
        }
        
        public void DogAnimationUpdate(DogAnimation anim)
        {
            TrackEntry entry = null;
            // print("switching from animation " + _curAnim +" to animation " + anim);
            switch (anim)
            {
                case DogAnimation.Idle:
                    entry = spineAnimationState.SetAnimation(0, idleAnimName, true);
                    if (entry != null) entry.TimeScale = idleAnimSpeed;
                    break;
                case DogAnimation.Walk:
                    entry = spineAnimationState.SetAnimation(0, walkAnimName, true);
                    if (entry != null) entry.TimeScale = walkAnimSpeed;
                    break;
                case DogAnimation.Run:
                    entry = spineAnimationState.SetAnimation(0, runAnimName, true);
                    if (entry != null) entry.TimeScale = runAnimSpeed;
                    break;
                case DogAnimation.Growl:
                    entry = spineAnimationState.SetAnimation(0, growlAnimName, false);
                    if (entry != null) entry.TimeScale = growlAnimSpeed;
                    break;
                case DogAnimation.Eat:
                    entry = spineAnimationState.SetAnimation(0, eatAnimName, false);
                    if (entry != null) entry.TimeScale = eatAnimSpeed;
                    break;
                case DogAnimation.Sniff:
                    entry = spineAnimationState.SetAnimation(0, sniffAnimName, true);
                    if (entry != null) entry.TimeScale = sniffAnimSpeed;
                    break;
                case DogAnimation.Listen:
                    entry = spineAnimationState.SetAnimation(0, listenAnimName, true);
                    if (entry != null) entry.TimeScale = listenAnimSpeed;
                    break;
                case DogAnimation.IdleCrouch:
                    entry = spineAnimationState.SetAnimation(0, idleCrouchAnimName, true);
                    if (entry != null) entry.TimeScale = idleCrouchAnimSpeed;
                    break;
                case DogAnimation.WalkCrouch:
                    entry = spineAnimationState.SetAnimation(0, walkCrouchAnimName, true);
                    if (entry != null) entry.TimeScale = walkCrouchAnimSpeed;
                    break;
                case DogAnimation.HappyCrouch:
                    entry = spineAnimationState.SetAnimation(0, happyCrouchAnimName, true);
                    if (entry != null) entry.TimeScale = happyCrouchAnimSpeed;
                    break;
                case DogAnimation.Jump:
                    entry = spineAnimationState.SetAnimation(0, jumpAnimName, false);
                    if (entry != null) entry.TimeScale = jumpAnimSpeed;
                    break;
                case DogAnimation.Dig:
                    entry = spineAnimationState.SetAnimation(0, digAnimName, true);
                    if (entry != null) entry.TimeScale = digAnimSpeed;
                    break;
                case DogAnimation.Pet:
                    entry = spineAnimationState.SetAnimation(0, petAnimName, true);
                    if (entry != null) entry.TimeScale = petAnimSpeed;
                    FaceTowardsTransform(playerTransform);
                    break;
                case DogAnimation.AfterEat:
                    entry = spineAnimationState.SetAnimation(0, afterEatAnimName, false);
                    if (entry != null) entry.TimeScale = afterEatAnimSpeed;
                    break;
                case DogAnimation.AirSniff:
                    entry = spineAnimationState.SetAnimation(0, airSniffAnimName, true);
                    
                    if (entry != null) entry.TimeScale = airSniffAnimSpeed;
                    break;
                case DogAnimation.TailWag:
                    entry = spineAnimationState.SetAnimation(0, tailWagAnimName, true);
                    if (entry != null) entry.TimeScale = tailWagAnimSpeed;
                    break;
            }

            _curAnim = anim;
        }
        
        private void FaceTowardsPosition(Vector2 position)
        {
            float direction = position.x - transform.position.x;
            float newScaleX = direction > 0 ? 1 : -1;

            art.transform.localScale = new Vector3(
                newScaleX * Mathf.Abs(art.transform.localScale.x),
                art.transform.localScale.y,
                art.transform.localScale.z
            );
        }
        
        private void FaceTowardsTransform(Transform FaceTo)
        {
            float direction = FaceTo.position.x - transform.position.x;
            float newScaleX = direction > 0 ? 1 : -1;

            art.transform.localScale = new Vector3(
                newScaleX * Mathf.Abs(art.transform.localScale.x),
                art.transform.localScale.y,
                art.transform.localScale.z
            );
        }
    }
}