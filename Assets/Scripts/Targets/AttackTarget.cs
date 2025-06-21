// using System.Collections.Generic;

using System.Collections;
using Audio.FMOD;
using Dog;
using UnityEngine;

namespace Targets
{
    public class AttackTarget : Target
    {
        [SerializeField] private float _waitToFinishInteract = 1f;
        
        private IEnumerator WaitToFinish()
        {
            yield return new WaitForSeconds(_waitToFinishInteract);
            FinishTargetAction();
        }
        
        public override void StartTargetAction(PlayerFollower dog)
        {
            DogActionManager manager = dog.GetComponent<DogActionManager>();
            
            manager.Growl(transform, false);
            manager.Running(false);

            StartCoroutine(WaitToFinish());
        }
    }
}