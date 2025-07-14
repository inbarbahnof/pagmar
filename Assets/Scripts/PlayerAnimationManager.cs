using System;
using System.Collections;
using Audio.FMOD;
using DG.Tweening;
using FMOD.Studio;
using Interactables;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] SkeletonAnimation skeletonAnimation;
    [SerializeField] private Transform heldObject;
    [SerializeField] private GameObject _holdingHand;
    [SerializeField] private GameObject _playerArt;
    [SerializeField] private Animator _shadowAnimator;
    
    [Header("Animation Names")]
    [SerializeField] private string idleAnimName;
    [SerializeField] private string callAnimName;
    [SerializeField] private string pushAnimName;
    [SerializeField] private string pullAnimName;
    [SerializeField] private string runAnimName;
    [SerializeField] private string pickUpAnimName;
    [SerializeField] private string climbAnimName;
    [SerializeField] private string crouchIdleAnimName;
    [SerializeField] private string crouchWalkAnimName;
    [SerializeField] private string throwAnimName;
    [SerializeField] private string aimAnimName;
    [SerializeField] private string petAnimName;
    [SerializeField] private string cantPetAnimName;
    [SerializeField] private string dropAnimName;
    [SerializeField] private string narrowPassAnimName;
    [SerializeField] private string holdPushAnimName;
    [SerializeField] private string sadWalkAnimName;
    [SerializeField] private string sadStopAnimName;
    [SerializeField] private string scaredAnimName;
    
    [Header("Animation Speeds")]
    [SerializeField] private float idleAnimSpeed = 1f;
    [SerializeField] private float callAnimSpeed = 2f;
    [SerializeField] private float pushAnimSpeed = 1f;
    [SerializeField] private float pullAnimSpeed = 1f;
    [SerializeField] private float runAnimSpeed = 1.25f;
    [SerializeField] private float pickUpAnimSpeed = 1f;
    [SerializeField] private float climbAnimSpeed = 1f;
    [SerializeField] private float crouchIdleAnimSpeed = 1f;
    [SerializeField] private float crouchWalkAnimSpeed = 1f;
    [SerializeField] private float throwAnimSpeed = 1f;
    [SerializeField] private float aimAnimSpeed = 1f;
    [SerializeField] private float petAnimSpeed = 1f;
    [SerializeField] private float cantPetAnimSpeed = 1f;
    [SerializeField] private float dropAnimSpeed = 1f;
    [SerializeField] private float narrowPassAnimSpeed = 1f;
    [SerializeField] private float holdPushAnimSpeed = 1f;
    [SerializeField] private float sadWalkAnimSpeed = 1f;
    [SerializeField] private float sadStopAnimSpeed = 1f;
    [SerializeField] private float scaredAnimSpeed = 1f;

    [Header("Event Names")] 
    [SerializeField] private string _climbUpEventName;
    [SerializeField] private string _climbRightEventName;
    [SerializeField] private string _footstepsEventName;
    [SerializeField] private string _pickUpEventName;
    [SerializeField] private string _breathEventName;
    [SerializeField] private string _callEventName;
    [SerializeField] private string _scareEventName;
    
    private Spine.EventData _climbUpEventData;
    private Spine.EventData _climbRightEventData;
    private Spine.EventData _footstepsEventData;
    private Spine.EventData _pickUpEventData;
    private Spine.EventData _breathEventData;
    private Spine.EventData _callEventData;
    private Spine.EventData _scareEventData;

    private PlayerAnimation _curAnim;
    private bool _isHolding;
    private float _lastFacingDirection = 1f;

    private string _holdBoneName = "F Arm Palm";
    private Bone handBone;
    
    private Spine.AnimationState spineAnimationState;
    private Spine.Skeleton skeleton;

    private bool _climbDropRight = true;
    private PlayerStateManager _stateManager;

    private ParticleSystem _leafsParticals;
    
    void Start()
    {
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
        
        _climbUpEventData = skeletonAnimation.Skeleton.Data.FindEvent(_climbUpEventName);
        _climbRightEventData = skeletonAnimation.Skeleton.Data.FindEvent(_climbRightEventName);
        _footstepsEventData = skeletonAnimation.Skeleton.Data.FindEvent(_footstepsEventName);
        _pickUpEventData = skeletonAnimation.Skeleton.Data.FindEvent(_pickUpEventName);
        _breathEventData = skeletonAnimation.Skeleton.Data.FindEvent(_breathEventName);
        _callEventData = skeletonAnimation.Skeleton.Data.FindEvent(_callEventName);
        _scareEventData = skeletonAnimation.Skeleton.Data.FindEvent(_scareEventName);
        
        skeletonAnimation.AnimationState.Event += HandleAnimationStateEvent;

        handBone = skeletonAnimation.Skeleton.FindBone(_holdBoneName);

        _stateManager = GetComponent<PlayerStateManager>();
    }

    public void UpdateClimbDropDirection(bool right)
    {
        _climbDropRight = right;
    }
    
    private void HandleAnimationStateEvent (TrackEntry trackEntry, Spine.Event e) {
        if (e.Data == _climbUpEventData)
            OnClimbUpEvent();
        else if (e.Data == _climbRightEventData)
            OnClimbRightEvent();
        else if (e.Data == _footstepsEventData)
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.PlayerFootsteps,
                transform.position, true);
            if (_leafsParticals != null) _leafsParticals.Play();
        }
        else if (e.Data == _pickUpEventData)
            PickUpInteractableManager.instance.PickUpCurrentObject();
        else if (e.Data == _callEventData)
            _stateManager.MakeCallSound();
        else if (e.Data == _breathEventData)
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.PlayerSigh, 
                transform.position, true);
        else if (e.Data == _scareEventData)
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.PlayerScared, 
                transform.position, true);
    }

    private void SetFlipSprite(float direction)
    {
        _playerArt.transform.localScale = new Vector3(
            direction * Mathf.Abs(skeletonAnimation.transform.localScale.x),
            skeletonAnimation.transform.localScale.y,
            skeletonAnimation.transform.localScale.z
        );
    }
    
    public void FlipSpriteBasedOnInput(Vector2 moveInput, Vector2 aimInput)
    {
        float newScaleX = _playerArt.transform.localScale.x;

        if (Mathf.Abs(moveInput.x) > 0.001f)
        {
            newScaleX = moveInput.x > 0 ? 1 : -1;
        }
        else if (Mathf.Abs(aimInput.x) > 0.001f)
        {
            newScaleX = aimInput.x > 0 ? 1 : -1;
        }
    
        SetFlipSprite(newScaleX);
    }
    
    public void FaceTowardsTransform(Transform target)
    {
        if (target == null) return;

        float direction = target.position.x - transform.position.x;
        float newScaleX = direction >= 0 ? 1 : -1;

        SetFlipSprite(newScaleX);
    }

    public void FaceAgainstTransform(Transform target)
    {
        if (target == null) return;

        float direction = target.position.x - transform.position.x;
        float newScaleX = direction >= 0 ? -1 : 1;

        SetFlipSprite(newScaleX);
    }

    public void SetLeafsParticles(ParticleSystem leaf)
    {
        _leafsParticals = leaf;
    }

    private void LateUpdate()
    {
        if(_curAnim != PlayerAnimation.Aim && _isHolding)
        {
            Vector3 worldPos = skeletonAnimation.transform.TransformPoint(new Vector3(handBone.WorldX, handBone.WorldY - 0.1f, 0));
            heldObject.position = worldPos;
        }
        else 
        {
            Vector3 worldPos = skeletonAnimation.transform.TransformPoint(new Vector3(handBone.WorldX, handBone.WorldY + 0.1f, 0));
            heldObject.position = worldPos;
        }
    }
    
    private void OnClimbUpEvent()
    {
        float deltaX = _climbDropRight ? 0.1f : -0.1f;
        Vector3 newPos = new Vector3(transform.position.x + deltaX, transform.position.y + 0.3f, 0);
        transform.DOMove(newPos, 0.53f);
        _shadowAnimator.SetTrigger("out");
    }
    
    private void OnClimbRightEvent()
    {
        float deltaX = _climbDropRight ? 0.3f : -0.3f;
        Vector3 newPos = new Vector3(transform.position.x + deltaX, transform.position.y + 0.1f, 0);
        transform.DOMove(newPos, 0.5f);
        _shadowAnimator.SetTrigger("in");
    }

    private IEnumerator OnDrop()
    {
        float deltaX1 = _climbDropRight ? 0.4f : -0.4f;
        float deltaX2 = _climbDropRight ? 0.3f : -0.3f;
        
        _shadowAnimator.SetTrigger("outNoTrans");
        Vector3 newPos1 = new Vector3(transform.position.x + deltaX1, transform.position.y + 0.1f, 0);
        transform.DOMove(newPos1, 0.15f);

        yield return new WaitForSeconds(0.15f);
        
        Vector3 newPos2 = new Vector3(transform.position.x + deltaX2, transform.position.y - 0.5f, 0);
        transform.DOMove(newPos2, 0.35f);
        _shadowAnimator.SetTrigger("in");
    }

    public void PlayerRunSpeed(bool isFast)
    {
        if (isFast) runAnimSpeed = 1.5f;
        else runAnimSpeed = 1.25f;
    }

    public void StartIdleAnim()
    {
        TrackEntry entry = spineAnimationState.SetAnimation(0, idleAnimName, true);
        if (entry != null) entry.TimeScale = idleAnimSpeed;
        _curAnim = PlayerAnimation.Idle;
    }
    
    public void PlayerAnimationUpdate(PlayerAnimation anim)
    {
        
        if (_curAnim != anim)
        {
            TrackEntry entry = null;
            // print("switching from animation " + _curAnim +" to animation " + animation);
            switch (anim)
            {
                case PlayerAnimation.Idle:
                    entry = spineAnimationState.SetAnimation(0, idleAnimName, true);
                    if (entry != null) entry.TimeScale = idleAnimSpeed;
                    break;
                case PlayerAnimation.Call:
                    entry = spineAnimationState.SetAnimation(0, callAnimName, false);
                    if (entry != null) entry.TimeScale = callAnimSpeed;
                    break;
                case PlayerAnimation.Push:
                    entry = spineAnimationState.SetAnimation(0, pushAnimName, true);
                    if (entry != null) entry.TimeScale = pushAnimSpeed;
                    break;
                case PlayerAnimation.Pull:
                    entry = spineAnimationState.SetAnimation(0, pullAnimName, true);
                    if (entry != null) entry.TimeScale = pullAnimSpeed;
                    break;
                case PlayerAnimation.PickUp:
                    _isHolding = !_isHolding;
                    _holdingHand.SetActive(_isHolding);
                    entry = spineAnimationState.SetAnimation(0, pickUpAnimName, false);
                    spineAnimationState.AddAnimation(0, idleAnimName, true, 0);
                    if (entry != null) entry.TimeScale = pickUpAnimSpeed;
                    break;
                case PlayerAnimation.Climb:
                    entry = spineAnimationState.SetAnimation(0, climbAnimName, false);
                    if (entry != null) entry.TimeScale = climbAnimSpeed;
                    break;
                case PlayerAnimation.Scared:
                    entry = spineAnimationState.SetAnimation(0, scaredAnimName, false);
                    if (entry != null) entry.TimeScale = scaredAnimSpeed;
                    break;
                case PlayerAnimation.Run:
                    entry = spineAnimationState.SetAnimation(0, runAnimName, true);
                    if (entry != null) entry.TimeScale = runAnimSpeed;
                    break;
                case PlayerAnimation.CrouchIdle:
                    entry = spineAnimationState.SetAnimation(0, crouchIdleAnimName, true);
                    if (entry != null) entry.TimeScale = crouchIdleAnimSpeed;
                    break;
                case PlayerAnimation.CrouchWalk:
                    entry = spineAnimationState.SetAnimation(0, crouchWalkAnimName, true);
                    if (entry != null) entry.TimeScale = crouchWalkAnimSpeed;
                    break;
                case PlayerAnimation.Aim:
                    _holdingHand.SetActive(false);
                    entry = spineAnimationState.SetAnimation(0, aimAnimName, true);
                    if (entry != null) entry.TimeScale = aimAnimSpeed;
                    break;
                case PlayerAnimation.Throw:
                    _holdingHand.SetActive(false);
                    _isHolding = false;
                    
                    entry = spineAnimationState.SetAnimation(0, throwAnimName, false);
                    spineAnimationState.AddAnimation(0, idleAnimName, true, 0);
                    if (entry != null) entry.TimeScale = throwAnimSpeed;
                    break;
                case PlayerAnimation.Pet:
                    entry = spineAnimationState.SetAnimation(0, petAnimName, true);
                    if (entry != null) entry.TimeScale = petAnimSpeed;
                    break;
                case PlayerAnimation.GoBackFromPet:
                    entry = spineAnimationState.SetAnimation(0, cantPetAnimName, false);
                    if (entry != null) entry.TimeScale = cantPetAnimSpeed;
                    break;
                case PlayerAnimation.NarrowPass:
                    entry = spineAnimationState.SetAnimation(0, narrowPassAnimName, true);
                    if (entry != null) entry.TimeScale = narrowPassAnimSpeed;
                    break;
                case PlayerAnimation.HoldPush:
                    entry = spineAnimationState.SetAnimation(0, holdPushAnimName, true);
                    if (entry != null) entry.TimeScale = holdPushAnimSpeed;
                    break;
                case PlayerAnimation.Drop:
                    entry = spineAnimationState.SetAnimation(0, dropAnimName, false);
                    if (entry != null) entry.TimeScale = dropAnimSpeed;
                    StartCoroutine(OnDrop());
                    break;
                case PlayerAnimation.SadWalk:
                    entry = spineAnimationState.SetAnimation(0, sadWalkAnimName, true);
                    if (entry != null) entry.TimeScale = sadWalkAnimSpeed;
                    break;
                case PlayerAnimation.StopSad:
                    entry = spineAnimationState.SetAnimation(0, sadStopAnimName, false);
                    if (entry != null) entry.TimeScale = sadStopAnimSpeed;
                    break;
            }

            _curAnim = anim;
        }
    }
}
