using UnityEngine;

namespace Targets
{
    public class WantFoodTarget : Target
    {
        public override void StartTargetAction()
        {
            print("Dog reached want food");
            if (_dogTempComunication != null)
                _dogTempComunication.SetActive(true);
        }

        public void FoodCanBeReached()
        {
            FinishTargetAction();
        }
    }
}