using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Targets
{
    public class HoverTarget : Target
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
        }
    }
}
