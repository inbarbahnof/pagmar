using System;
using UnityEngine;

namespace Interactabels
{
    public class PickUpInteractableManager : MonoBehaviour
    {
        public static PickUpInteractableManager instance;
        
        public event Action OnReachedTarget;

        [SerializeField] private Transform player;

        private Transform _pickUpParent;
        private Vector2 _carryTarget;
        
        void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else Debug.LogError("TOO MANY PICKUP INTERACTABLE MANAGERS!");
            
            _pickUpParent = player.GetComponent<PickUpParentManager>().GetPickUpParent();
        }
        
        public void SetCarryTarget(Vector2 target, Action onReachEvent)
        {
            _carryTarget = target;
            OnReachedTarget += onReachEvent;
        }

        public void Interact(PickUpInteractable pickup)
        {
            if (!pickup.IsPickedUp)
            {
                pickup.PickUpObject(_pickUpParent);
            }
            else
            {
                pickup.DropObject();
                if (Vector2.Distance(pickup.transform.position, _carryTarget) <= 0.5f)
                {
                    OnReachedTarget?.Invoke();
                }
            }
        }
    }
}