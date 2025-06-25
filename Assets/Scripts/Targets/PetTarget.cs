using Dog;
using UnityEngine;

namespace Targets
{
    public class PetTarget : HoverTarget
    {
        [SerializeField] private PlayerStateManager _player;
        
        public override void StartTargetAction(PlayerFollower dog)
        {
            _dog = dog.GetComponent<DogAnimationManager>();
            _player.StartPetting();
                
            if(GameManager.instance.ConnectionState > 3) _dog.PetBehavior();
            else _dog.DogGrowl(transform, true);
            
            StartCoroutine(base.HoverOverTarget());
        }
    }
}