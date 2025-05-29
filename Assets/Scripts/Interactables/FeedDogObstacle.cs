using System;
using System.Collections;
using Dog;
using Targets;
using UnityEngine;

namespace Interactables
{
    public class FeedDogObstacle : Obstacle
    {
        [SerializeField] private PickUpInteractable _food;
        [SerializeField] private WantFoodTarget _wantFoodTarget;
        
        private void Awake()
        {
            if (_food is FoodPickUpInteractable foodPickup)
            {
                foodPickup.OnDroppedOnWalkableSurface += HandleFoodDroppedWalkable;
            }

            // StartCoroutine(SetListener());
            FoodTarget foodTarget = _food.GetComponent<FoodTarget>();
            if (foodTarget != null)
            {
                foodTarget.OnFoodEaten += HandleFoodEaten;
            }
        }

        private void HandleFoodEaten()
        {
            GetComponent<Collider2D>().enabled = false;
        }
        
        public void HandleFoodDroppedWalkable(FoodPickUpInteractable food)
        {
            _wantFoodTarget.FoodCanBeReached();
        }

        public override void ResetObstacle()
        {
            _food.ResetToCheckpoint();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Dog"))
            {
                TargetGenerator.instance.SetWantFoodTarget(_wantFoodTarget);
                other.GetComponent<DogActionManager>().SetWantsFood(true);
            }
        }
    }
}
