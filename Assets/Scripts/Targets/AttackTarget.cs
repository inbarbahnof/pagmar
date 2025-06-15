// using System.Collections.Generic;

using System.Collections;
using Audio.FMOD;
using Dog;
using UnityEngine;

namespace Targets
{
    public class AttackTarget : Target
    {
        public override void StartTargetAction(PlayerFollower dog)
        {
            DogActionManager manager = dog.GetComponent<DogActionManager>();
            
            manager.Growl();
            manager.Running(false);
            
            FinishTargetAction();
        }
    }
}