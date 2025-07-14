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
            // turn off the pickability
            if (_pickUp != null)
            {
                InteractableManager.instance.RemoveInteractable(_pickUp);
                _pickUp.SetCanInteract(false);
            }
            
            _collider.enabled = false;
            
            dog.GetComponent<DogAnimationManager>().DogEat(_art.transform, 
                () => {_art.gameObject.SetActive(false);}
                ,() => {
                OnFoodEaten?.Invoke();
                FinishTargetAction();
                canBeFed = false;
                });
        }

        public void SetCantPickUp()
        {
            // turn off the pickability
            if (_pickUp != null)
            {
                InteractableManager.instance.RemoveInteractable(_pickUp);
                _pickUp.SetCanInteract(false);
            }
            
            _collider.enabled = false;
        }

        public PickUpInteractable GetPickup()
        {
            return _pickUp;
        }

        public void SetCanBeFed(bool can)
        {
            canBeFed = can;
        }
    }
}
