using UnityEngine;

namespace Interactables
{
    public class PickUpAndClimbObstacle : Obstacle
    {
        [SerializeField] private PickUpInteractableManager pickUpManager;
        [SerializeField] private GameObject roadBlock;
        private bool _setupComplete;
        private Vector2 _carryTarget;

        void Start()
        {
            _carryTarget = transform.position;
            pickUpManager.SetCarryTarget(_carryTarget, ReachedTarget, PickedUp);
        }

        public void ReachedTarget()
        {
            _setupComplete = true;
            roadBlock.GetComponent<Collider2D>().enabled = false;
        }

        public void PickedUp()
        {
            _setupComplete = false;
            roadBlock.GetComponent<Collider2D>().enabled = true;
        }
    
    }
}
