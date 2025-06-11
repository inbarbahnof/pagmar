// using System.Collections.Generic;

using System.Collections;
using Audio.FMOD;
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

        public override void StartTargetAction()
        {
            StartCoroutine(HoverOverTarget());
            print("attack target behavior");
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.DogBark);
        }
    }
}