using UnityEngine;

namespace Interactables
{
    public class PickUpAndClimbObstacle : Obstacle
    {
        [SerializeField] private PickUpInteractableManager pickUpManager;
        [SerializeField] private GameObject roadBlock;
        private Vector2 _carryTarget;

        void Start()
        {
            _carryTarget = transform.position;
            pickUpManager.SetCarryTarget(_carryTarget, ReachedTarget, PickedUp);
        }

        public void ReachedTarget()
        {
            base.ReachedTarget();
            roadBlock.GetComponent<Collider2D>().enabled = false;
        }

        public void PickedUp()
        {
            SetupComplete = false;
            roadBlock.GetComponent<Collider2D>().enabled = true;
        }
    
    }
}
