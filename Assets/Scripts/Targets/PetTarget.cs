using System;
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
            _player.StartPetting();
                
            if(GameManager.instance.ConnectionState > 3) _dog.PetBehavior();
            else _dog.DogGrowl(transform, true);
            
            StartCoroutine(base.HoverOverTarget());
            onPetEvent?.Invoke();
        }
    }
}