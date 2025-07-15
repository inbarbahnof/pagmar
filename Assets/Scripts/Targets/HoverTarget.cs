using System.Collections;
using System.Collections.Generic;
using Dog;
using UnityEngine;

namespace Targets
{
    public class HoverTarget : Target
    {
        [SerializeField] private float _hoverTime = 2f;
        
        protected DogAnimationManager _dog;
        
        protected IEnumerator HoverOverTarget()
        {
            yield return new WaitForSeconds(_hoverTime);
            
            if (isTOI) _dog.DogFinishSniff();
            
            FinishTargetAction();
        }

        public override void StartTargetAction(PlayerFollower dog)
        {
            if (isTOI)
            {
                _dog = dog.GetComponent<DogAnimationManager>();
                _dog.DogStartSniff();
            }
            
            if (gameObject.activeInHierarchy) StartCoroutine(HoverOverTarget());
            else
            {
                if (isTOI) _dog.DogFinishSniff();
                FinishTargetAction();
            }
        }
    }
}
