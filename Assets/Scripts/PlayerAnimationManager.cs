using System.Collections;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] SkeletonAnimation skeletonAnimation;
    
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
    [SerializeField] private string petAnimName;
    
    private PlayerAnimation _curAnim;
    
    private Spine.AnimationState spineAnimationState;
    private Spine.Skeleton skeleton;
    
    void Start()
    {
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
    }

    public void PlayerAnimationUpdate(PlayerAnimation animation)
    {
        if (_curAnim != animation)
        {
            TrackEntry entry = null;
            // print("switching from animation " + _curAnim +" to animation " + animation);
            switch (animation)
            {
                case PlayerAnimation.Idle:
                    entry = spineAnimationState.SetAnimation(0, idleAnimName, true);
                    break;
                case PlayerAnimation.Call:
                    entry = spineAnimationState.SetAnimation(0, callAnimName, false);
                    if (entry != null)
                        entry.TimeScale = 2f;
                    break;
                case PlayerAnimation.Push:
                    entry = spineAnimationState.SetAnimation(0, pushAnimName, true);
                    break;
                case PlayerAnimation.Pull:
                    entry = spineAnimationState.SetAnimation(0, pullAnimName, true);
                    break;
                case PlayerAnimation.PickUp:
                    entry = spineAnimationState.SetAnimation(0, pickUpAnimName, false);
                    spineAnimationState.AddAnimation(0, idleAnimName, true, 0);
                    break;
                case PlayerAnimation.Climb:
                    entry = spineAnimationState.SetAnimation(0, climbAnimName, false);
                    spineAnimationState.AddAnimation(0, idleAnimName, true, 0);
                    break;
                case PlayerAnimation.Run:
                    entry = spineAnimationState.SetAnimation(0, runAnimName, true);
                    if (entry != null)
                        entry.TimeScale = 1.25f;
                    break;
                case PlayerAnimation.CrouchIdle:
                    entry = spineAnimationState.SetAnimation(0, crouchIdleAnimName, true);
                    break;
                case PlayerAnimation.CrouchWalk:
                    entry = spineAnimationState.SetAnimation(0, crouchWalkAnimName, true);
                    break;
                case PlayerAnimation.Walking:
                    entry = spineAnimationState.SetAnimation(0, walkingAnimName, true);
                    break;
                case PlayerAnimation.Throw:
                    entry = spineAnimationState.SetAnimation(0, throwAnimName, false);
                    spineAnimationState.AddAnimation(0, idleAnimName, true, 0);
                    break;
                case PlayerAnimation.Pet:
                    entry = spineAnimationState.SetAnimation(0, petAnimName, false);
                    spineAnimationState.AddAnimation(0, idleAnimName, true, 0);
                    break;
            }

            if (animation != PlayerAnimation.Run && animation != PlayerAnimation.Call && entry != null)
            {
                entry.TimeScale = 1f;
            }

            _curAnim = animation;
        }
    }
}
