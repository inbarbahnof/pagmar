using UnityEngine;
using UnityEngine.Serialization;

namespace Interactables
{
    public class PushObstacle : Obstacle
    {
        [SerializeField] private PushInteractableManager pushManager;
        [SerializeField] private GameObject colMid;
        [SerializeField] private PushInteractable interactable;
        [SerializeField] private Transform pushTarget;
        [SerializeField] private ClimbPushableInteractable climbUpObject;
        [SerializeField] private bool _isJumping;
        

        void Start()
        {
            interactable.SetPushTarget(pushTarget.position, ReachedTarget);
        }

        public override void ReachedTarget()
        {
            base.ReachedTarget();
            SwapToClimb(true);
            DisableBarrier(true);
        }

        public override void ResetObstacle()
        {
            pushManager.ResetToCheckpoint(interactable);
            SwapToClimb(false);
            DisableBarrier(false);
        }

        private void DisableBarrier(bool enable)
        {
            if (colMid != null) colMid.SetActive(!enable);
            interactable.gameObject.GetComponent<Collider2D>().enabled = !enable;
            if (!_isJumping) NavMeshManager.instance.ReBake();
        }

        private void SwapToClimb(bool climb)
        {
            if (climbUpObject)
            {
                climbUpObject.gameObject.SetActive(climb);
                InteractableManager.instance.RemoveInteractable(interactable);
                InteractableManager.instance.AddInteractableObj(climbUpObject);
                interactable.gameObject.SetActive(!climb);
            }
        }
    }
}
