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
    [SerializeField] private string walkingAnimName;
    [SerializeField] private string throwAnimName;
    [SerializeField] private string aimAnimName;
    [SerializeField] private string petAnimName;
    
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
    [SerializeField] private float walkingAnimSpeed = 1f;
    [SerializeField] private float throwAnimSpeed = 1f;
    [SerializeField] private float aimAnimSpeed = 1f;
    [SerializeField] private float petAnimSpeed = 1f;

    [Header("Event Names")] 
    [SerializeField] private string _climbUpEventName;
    [SerializeField] private string _climbRightEventName;
    [SerializeField] private string _footstepsEventName;
    [SerializeField] private string _pickUpEventName;
    
    private Spine.EventData _climbUpEventData;
    private Spine.EventData _climbRightEventData;
    private Spine.EventData _footstepsEventData;
    private Spine.EventData _pickUpEventData;
    
    private PlayerAnimation _curAnim;
    private EventInstance _dragSound;
    private bool _isHolding;

    private string _holdBoneName = "F Arm Palm";
    private Bone handBone;
    
    private Spine.AnimationState spineAnimationState;
    private Spine.Skeleton skeleton;
    
    void Start()
    {
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
        
        _climbUpEventData = skeletonAnimation.Skeleton.Data.FindEvent(_climbUpEventName);
        _climbRightEventData = skeletonAnimation.Skeleton.Data.FindEvent(_climbRightEventName);
        _footstepsEventData = skeletonAnimation.Skeleton.Data.FindEvent(_footstepsEventName);
        _pickUpEventData = skeletonAnimation.Skeleton.Data.FindEvent(_pickUpEventName);
        
        skeletonAnimation.AnimationState.Event += HandleAnimationStateEvent;
        
        handBone = skeletonAnimation.Skeleton.FindBone(_holdBoneName);
    }
    
    private void HandleAnimationStateEvent (TrackEntry trackEntry, Spine.Event e) {
        if (e.Data == _climbUpEventData)
        {
            OnClimbUpEvent();
        }
        else if (e.Data == _climbRightEventData)
        {
            OnClimbRightEvent();
        }
        else if (e.Data == _footstepsEventData)
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.PlayerFootsteps, 
                transform.position, true);
        }
        if (e.Data == _pickUpEventData)
        {
            print("pickup event");
            PickUpInteractableManager.instance.PickUpCurrentObject();
        }
    }
    
    void LateUpdate()
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
    
    private void OnClimbUpEvent() {
        Vector3 newPos = new Vector3(transform.position.x + 0.1f, transform.position.y + 0.3f, 0);
        transform.DOMove(newPos, 0.53f);
    }
    
    private void OnClimbRightEvent() {
        Vector3 newPos = new Vector3(transform.position.x + 0.3f, transform.position.y + 0.1f, 0);
        transform.DOMove(newPos, 0.5f);
    }

    public void PlayerRunSpeed(bool isFast)
    {
        if (isFast) runAnimSpeed = 1.5f;
        else runAnimSpeed = 1.25f;
    }
    
    public void PlayerAnimationUpdate(PlayerAnimation anim)
    {
        if (_curAnim != anim)
        {
            if (_curAnim == PlayerAnimation.Pull || _curAnim == PlayerAnimation.Push)
            {
                AudioManager.Instance.StopSound(_dragSound);
                _dragSound = default;
            }
            
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
                    if (!_dragSound.isValid())
                        _dragSound = AudioManager.Instance.PlayLoopingSound(FMODEvents.Instance.PlayerDrag);
                    entry = spineAnimationState.SetAnimation(0, pushAnimName, true);
                    if (entry != null) entry.TimeScale = pushAnimSpeed;
                    break;
                case PlayerAnimation.Pull:
                    if (!_dragSound.isValid())
                        _dragSound = AudioManager.Instance.PlayLoopingSound(FMODEvents.Instance.PlayerDrag);
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
                case PlayerAnimation.Walking:
                    entry = spineAnimationState.SetAnimation(0, walkingAnimName, true);
                    if (entry != null) entry.TimeScale = walkingAnimSpeed;
                    break;
                case PlayerAnimation.Aim:
                    _holdingHand.SetActive(false);
                    entry = spineAnimationState.SetAnimation(0, aimAnimName, true);
                    if (entry != null) entry.TimeScale = aimAnimSpeed;
                    break;
                case PlayerAnimation.Throw:
                    _holdingHand.SetActive(false);
                    _isHolding = false;
                    AudioManager.Instance.PlayOneShot(FMODEvents.Instance.PlayerThrow);
                    entry = spineAnimationState.SetAnimation(0, throwAnimName, false);
                    spineAnimationState.AddAnimation(0, idleAnimName, true, 0);
                    if (entry != null) entry.TimeScale = throwAnimSpeed;
                    break;
                case PlayerAnimation.Pet:
                    entry = spineAnimationState.SetAnimation(0, petAnimName, false);
                    spineAnimationState.AddAnimation(0, idleAnimName, true, 0);
                    if (entry != null) entry.TimeScale = petAnimSpeed;
                    break;
            }

            _curAnim = anim;
        }
    }
}
