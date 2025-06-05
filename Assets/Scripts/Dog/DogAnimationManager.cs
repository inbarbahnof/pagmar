using System;
using System.Collections;
using System.Collections.Generic;
using Audio.FMOD;
using FMOD.Studio;
using Targets;
using UnityEngine;

namespace Dog
{
    public class DogAnimationManager : MonoBehaviour
    {
        [SerializeField] private GameObject art;

        private DogActionManager _actionManager;
        private Vector3 lastPosition;
        private float moveXPrevDir;

        private void Start()
        {
            _actionManager = GetComponent<DogActionManager>();
            lastPosition = transform.position;
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
    }
}