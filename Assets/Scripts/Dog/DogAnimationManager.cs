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

namespace Dog
{
    public class DogAnimationManager : MonoBehaviour
    {
        [SerializeField] SkeletonAnimation skeletonAnimation;

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
        [SerializeField] private float sniffAnimSpeed = 0.8f;
        [SerializeField] private float walkCrouchAnimSpeed = 0.8f;
        
        
        private DogAnimation _curAnim;
        
        private bool _isMoving;

        private Spine.AnimationState spineAnimationState;
        
        [SerializeField] private GameObject art;

        private DogActionManager _actionManager;
        private Vector3 lastPosition;
        private float moveXPrevDir;

        private void Start()
        {
            _actionManager = GetComponent<DogActionManager>();
            lastPosition = transform.position;
            spineAnimationState = skeletonAnimation.AnimationState;

            DogAnimationUpdate(DogAnimation.Idle);
        }

        private void Update()
        {
            CheckRotation();
            
            // animation state
            UpdateMoving();
            
            DogAnimation cur = WitchAnimShouldBePlayed();
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

            // if (_actionManager.CurState != DogState.Push)
            // {
            //     moveXPrevDir = dirX;
            //     lastPosition = currentPosition;
            // }
        }

        private void UpdateMoving()
        {
            Vector3 currentPosition = transform.position;
            float distanceMoved = Vector3.Distance(currentPosition, lastPosition);
            _isMoving = distanceMoved > 0.03f;
            lastPosition = currentPosition; 
        }

        private DogAnimation WitchAnimShouldBePlayed()
        {
            if (_isMoving)
            {
                if (_actionManager.IsRunning) return DogAnimation.Run;
                return DogAnimation.Walk;
            }
            
            return DogAnimation.Idle;
        }
        
        public IEnumerator DogBark()
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.DogBark, 
                transform.position, true);
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
            }

            _curAnim = anim;
        }
    }
}