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
        [SerializeField] private PickUpInteractable _pickUp;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private GameObject _art;
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
            _collider.enabled = false;
            
            // eat the target
            dog.GetComponent<DogAnimationManager>().DogEat(transform);
            StartCoroutine(EatTarget());
        }

        public PickUpInteractable GetPickup()
        {
            return _pickUp;
        }

        public void SetCanBeFed(bool can)
        {
            canBeFed = can;
        }
        
        private IEnumerator EatTarget()
        {
            yield return new WaitForSeconds(1.2f);
            
            OnFoodEaten?.Invoke();
            FinishTargetAction();
            _art.gameObject.SetActive(false);
        }
    }
}
