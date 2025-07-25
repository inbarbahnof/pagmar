﻿using Audio.FMOD;
using Dog;
using UnityEngine;

namespace Targets
{
    public class WantFoodTarget : Target
    {
        [SerializeField] private bool _isFoodClose;
        public bool IsFoodClose => _isFoodClose;
        
        private bool _foodCanBeReached = false;
        
        public override void StartTargetAction(PlayerFollower dog)
        {
            if (_foodCanBeReached) FinishTargetAction();
            
            if (_isFoodClose) dog.GetComponent<DogActionManager>().WaitUntilEating();
        }

        public void FoodCanBeReached()
        {
            _foodCanBeReached = true;
            FinishTargetAction();
        }
    }
}