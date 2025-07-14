using System;
using System.Collections;
using Dog;
using Targets;
using UnityEngine;

namespace Interactables
{
    public class FeedDogObstacle : Obstacle
    {
        [SerializeField] private FoodPickUpInteractable _food;
        [SerializeField] private WantFoodTarget _wantFoodTarget;
        [SerializeField] private SpriteFade _foodSpriteFade;

        private Coroutine _foodGlow;
        private bool _foodOnWalkable;
        
        private void Awake()
        {
            _food.OnDroppedOnWalkableSurface += HandleFoodDroppedWalkable;

            // StartCoroutine(SetListener());
            FoodTarget foodTarget = _food.GetComponent<FoodTargetGetter>().GetFoodTarget();
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
            if (_foodGlow != null) StopCoroutine(_foodGlow);

            _foodOnWalkable = true;
            _wantFoodTarget.FoodCanBeReached();
            _food.SetCanInteract(false);
            StartCoroutine(TurnOffInteractacle());
        }

        private IEnumerator TurnOffInteractacle()
        {
            yield return new WaitForSeconds(0.05f);
            InteractableManager.instance.RemoveInteractable(_food);
        }

        public void FoodGlow()
        {
            _foodGlow = StartCoroutine(FoodGlowCoroutine());
        }

        private IEnumerator FoodGlowCoroutine()
        {
            for (int i = 0; i < 3; i++)
            {
                _foodSpriteFade.FadeOutOverTime(true);
                yield return new WaitForSeconds(2f);
                _foodSpriteFade.FadeOutOverTime();
                yield return new WaitForSeconds(1f);
            }
        }
        
        public override void ResetObstacle()
        {
            _food.ResetToCheckpoint();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Dog") && !_foodOnWalkable)
            {
                TargetGenerator.instance.SetWantFoodTarget(_wantFoodTarget);
                DogActionManager dog = other.GetComponent<DogActionManager>();
                dog.SetWantsFood(true);
                
                if (_wantFoodTarget.IsFoodClose) 
                    dog.FoodIsClose(_food.GetComponent<Collider2D>());
            }
        }
    }
}
