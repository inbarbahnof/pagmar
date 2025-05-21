using UnityEngine;

namespace Targets
{
    public class WantFoodTarget : Target
    {
        private bool _foodCanBeReached = false;
        public override void StartTargetAction()
        {
            if (_foodCanBeReached) FinishTargetAction();
            
            print("Dog reached want food");
            if (_dogTempComunication != null)
                _dogTempComunication.SetActive(true);
        }

        public void FoodCanBeReached()
        {
            _foodCanBeReached = true;
            FinishTargetAction();
        }
    }
}