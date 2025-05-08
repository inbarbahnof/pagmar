using UnityEngine;

namespace Targets
{
    public class WantFoodTarget : Target
    {
        public override void StartTargetAction()
        {
            print("Dog reached want food");
        }

        public void FoodCanBeReached()
        {
            FinishTargetAction();
        }
    }
}