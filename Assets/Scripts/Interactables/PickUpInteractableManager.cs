using System;
using UnityEngine;

namespace Interactables
{
    public class PickUpInteractableManager : MonoBehaviour
    {
        public static PickUpInteractableManager instance;
        
        public event Action OnReachedTarget;
        public event Action OnPickedUp;

        [SerializeField] private Transform player;

        private Transform _pickUpParent;
        private Vector2 _carryTarget = Vector2.zero;
        
        void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else Debug.LogError("TOO MANY PICKUP INTERACTABLE MANAGERS!");
            
            _pickUpParent = player.GetComponent<PickUpParentManager>().GetPickUpParent();
        }
        
        public void SetCarryTarget(Vector2 target, Action onReachEvent, Action onPickedUp)
        {
            _carryTarget = target;
            OnReachedTarget += onReachEvent;
            OnPickedUp += onPickedUp;
        }

        public void Interact(PickUpInteractable pickup)
        {
            if (!pickup.IsPickedUp)
            {
                pickup.PickUpObject(_pickUpParent);
                OnPickedUp?.Invoke();
            }
            else
            {
                if (Vector2.Distance(pickup.transform.position, _carryTarget) <= 0.5f)
                {
                    pickup.DropObject(_carryTarget);
                    OnReachedTarget?.Invoke();
                }
                else
                {
                    pickup.DropObject(Vector2.zero);
                }
            }
        }
    }
}