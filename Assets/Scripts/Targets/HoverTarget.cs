using System.Collections;
using System.Collections.Generic;
using Dog;
using UnityEngine;

namespace Targets
{
    public class HoverTarget : Target
    {
        [SerializeField] private float _hoverTime = 2f;

        private DogAnimationManager _dog;
        
        private IEnumerator HoverOverTarget()
        {
            yield return new WaitForSeconds(_hoverTime);
            _dog.DogFinishSniff();
            FinishTargetAction();
        }

        public override void StartTargetAction(PlayerFollower dog)
        {
            _dog = dog.GetComponent<DogAnimationManager>();
            _dog.DogStartSniff();
            
            StartCoroutine(HoverOverTarget());
        }
    }
}
