// using System.Collections.Generic;

using System.Collections;
using Audio.FMOD;
using Dog;
using UnityEngine;

namespace Targets
{
    public class AttackTarget : Target
    {
        private IEnumerator WaitToFinish()
        {
            yield return new WaitForSeconds(1f);
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