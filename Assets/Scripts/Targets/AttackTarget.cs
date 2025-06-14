// using System.Collections.Generic;

using System.Collections;
using Audio.FMOD;
using Dog;
using UnityEngine;

namespace Targets
{
    public class AttackTarget : Target
    {
        [SerializeField] private float _hoverTime = 2f;
        
        private IEnumerator HoverOverTarget()
        {
            yield return new WaitForSeconds(_hoverTime);
            FinishTargetAction();
        }

        public override void StartTargetAction(PlayerFollower dog)
        {
            DogActionManager manager = dog.GetComponent<DogActionManager>();
            
            manager.Growl();
            manager.Running(false);
            
            if (gameObject.activeSelf) StartCoroutine(HoverOverTarget());
            else FinishTargetAction();
            
        }
    }
}