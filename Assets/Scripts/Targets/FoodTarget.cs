using System;
using System.Collections;
using Dog;
using UnityEngine;
using Interactables;

namespace Targets
{
    public class FoodTarget : Target
    {
        public event Action OnFoodEaten;
        
        [SerializeField] private bool canBeFed;
        public bool CanBeFed => canBeFed;
            
        public override void StartTargetAction(PlayerFollower dog)
        {
            // print("Dog started eating food");
            if (_dogTempComunication != null)
                _dogTempComunication.SetActive(true);
            
            // turn off the pickability
            FoodPickUpInteractable foodPickUpInteractable = GetComponent<FoodPickUpInteractable>();
            
            if (foodPickUpInteractable != null)
            {
                InteractableManager.instance.RemoveInteractable(foodPickUpInteractable);
                foodPickUpInteractable.SetCanInteract(false);
            }
            GetComponent<Collider2D>().enabled = false;
            
            // eat the target
            dog.GetComponent<DogAnimationManager>().DogEat();
            StartCoroutine(EatTarget());
        }

        public void SetCanBeFed(bool can)
        {
            canBeFed = can;
        }
        
        private IEnumerator EatTarget()
        {
            yield return new WaitForSeconds(0.8f);
            
            gameObject.SetActive(false);
            OnFoodEaten?.Invoke();
            FinishTargetAction();
        }
    }
}
