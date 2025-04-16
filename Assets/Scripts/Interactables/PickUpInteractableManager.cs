using UnityEngine;

namespace Interactabels
{
    public class PickUpInteractableManager : MonoBehaviour
    {
        public static PickUpInteractableManager instance;

        [SerializeField] private Transform player;

        private Transform _pickUpParent;
        
        void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else Debug.LogError("TOO MANY PICKUP INTERACTABLE MANAGERS!");
            
            _pickUpParent = player.GetComponent<PickUpParentManager>().GetPickUpParent();
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
            }
        }
    }
}