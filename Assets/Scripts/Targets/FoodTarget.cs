using System.Collections;
using UnityEngine;

namespace Targets
{
    public class FoodTarget : Target
    {
        public override void StartTargetAction()
        {
            print("Dog started eating food");
            StartCoroutine(EatTarget());
        }
        
        private IEnumerator EatTarget()
        {
            yield return new WaitForSeconds(1.5f);
            print("Dog finished eating food");
            Destroy(gameObject);
            FinishTargetAction();
        }
    }
}
