using System;
using System.Collections;
using System.Collections.Generic;
using Audio.FMOD;
using FMOD.Studio;
using Spine;
using Spine.Unity;
using Targets;
using UnityEditor.Rendering;
using UnityEngine;

namespace Dog
{
    public class DogAnimationManager : MonoBehaviour
    {
        [SerializeField] SkeletonAnimation skeletonAnimation;

        [Header("Animation Names")] 
        [SerializeField] private string idleAnimName;
        [SerializeField] private string walkAnimName;

        [Header("Animation Speeds")] 
        [SerializeField] private float idleAnimSpeed = 1f;
        [SerializeField] private float walkAnimSpeed = 1f;

        private DogAnimation _curAnim;

        private Spine.AnimationState spineAnimationState;
        
        [SerializeField] private GameObject art;

        private DogActionManager _actionManager;
        private Vector3 lastPosition;
        private float moveXPrevDir;

        private bool isMoving;

        private void Start()
        {
            _actionManager = GetComponent<DogActionManager>();
            lastPosition = transform.position;
            spineAnimationState = skeletonAnimation.AnimationState;
        }

        private void Update()
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

            if (_actionManager.CurState != DogState.Push)
            {
                moveXPrevDir = dirX;
                lastPosition = currentPosition;
            }
        }

        public void UpdateMoving(bool moving)
        {
            DogAnimation newState = moving ? DogAnimation.Walk : DogAnimation.Idle;
            if (newState != _curAnim) DogAnimationUpdate(newState);
        }
        
        public IEnumerator DogBark()
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.DogBark);
            yield return new WaitForSeconds(0.3f);
        }
        
        public IEnumerator DogGrowl(float timeToGrowl)
        {
            EventInstance sound = AudioManager.Instance.PlayLoopingSound(FMODEvents.Instance.DogGrowl,
                transform.position, true);
            
            yield return new WaitForSeconds(timeToGrowl);
            
            AudioManager.Instance.StopSound(sound);
        }
        
        public void DogAnimationUpdate(DogAnimation anim)
        {
            if (_curAnim != anim)
            {
                TrackEntry entry = null;
                print("switching from animation " + _curAnim +" to animation " + anim);
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
                }

                _curAnim = anim;
            }
        }
    }
}