using Audio.FMOD;
using Dog;
using UnityEngine;

namespace Targets
{
    public class WantFoodTarget : Target
    {
        [SerializeField] private bool _isFoodClose;
        
        private bool _foodCanBeReached = false;
        public override void StartTargetAction(PlayerFollower dog)
        {
            if (_foodCanBeReached) FinishTargetAction();
            
            print("Dog reached want food");
            if (_dogTempComunication != null)
                _dogTempComunication.SetActive(true);
            
            if (_isFoodClose) AudioManager.Instance.PlayOneShot(FMODEvents.Instance.DogBark,
                dog.transform.position, true);
        }

        public void FoodCanBeReached()
        {
            _foodCanBeReached = true;
            FinishTargetAction();
        }
    }
}