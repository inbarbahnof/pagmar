using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Targets
{
    public class HoverTarget : Target
    {
        private IEnumerator HoverOverTarget()
        {
            yield return new WaitForSeconds(2f);
            FinishTargetAction();
        }

        public override void StartTargetAction()
        {
            StartCoroutine(HoverOverTarget());
        }
    }
}
