using System;
using System.Collections;
using Audio.FMOD;
using Dog;
using UnityEngine;
using UnityEngine.Events;

namespace Targets
{
    public class PetTarget : HoverTarget
    {
        [SerializeField] private PlayerStateManager _player;
        [SerializeField] private UnityEvent onPetEvent;
        
        public override void StartTargetAction(PlayerFollower dog)
        {
            _dog = dog.GetComponent<DogAnimationManager>();
            if ( GameManager.instance.ConnectionState > 3
                  && Vector2.Distance(dog.transform.position, transform.position) > 0.15f)
            {
                SmoothMover dogMover = dog.GetComponent<SmoothMover>();
                Vector3 offset = Vector3.zero;
                if (transform.position.x < _player.transform.position.x)
                {
                    offset = new Vector3(0.2f, 0, 0);
                }

                dogMover.MoveTo(transform.position + offset);
                StartCoroutine(GoToPetPos());
            }
            else PetBehavior();
        }

        private void PetBehavior()
        {
            _player.StartPetting();
                
            if(GameManager.instance.ConnectionState > 3) _dog.PetBehavior();
            else _dog.DogGrowl(transform, true);
            
            StartCoroutine(HoverOverTarget());
            onPetEvent?.Invoke();
            StartCoroutine(ResumeAmbienceSound());
        }
        
        private IEnumerator GoToPetPos()
        {
            yield return new WaitForSeconds(0.5f);
            PetBehavior();
        }

        private IEnumerator ResumeAmbienceSound()
        {
            yield return new WaitForSeconds(7f);
           
            AudioManager.Instance.ResumeAmbience();

        }
    }
}