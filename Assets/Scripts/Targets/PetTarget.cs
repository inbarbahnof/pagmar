using System;
using System.Collections;
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
            SmoothMover dogMover = dog.GetComponent<SmoothMover>();
            dogMover.MoveTo(transform.position);
            _dog = dog.GetComponent<DogAnimationManager>();

            StartCoroutine(GoToPetPos());
        }

        private IEnumerator GoToPetPos()
        {
            yield return new WaitForSeconds(0.5f);
            
            _player.StartPetting();
                
            if(GameManager.instance.ConnectionState > 3) _dog.PetBehavior();
            else _dog.DogGrowl(transform, true);
            
            StartCoroutine(HoverOverTarget());
            onPetEvent?.Invoke();
        }
    }
}