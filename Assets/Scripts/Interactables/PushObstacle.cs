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
        [SerializeField] private GameObject climbUpObject;
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
            print("disable");
            if (colMid != null) colMid.SetActive(!enable);
            interactable.gameObject.GetComponent<Collider2D>().enabled = !enable;
            if (!_isJumping) NavMeshManager.instance.ReBake();
        }

        private void SwapToClimb(bool climb)
        {
            if (climbUpObject)
            {
                climbUpObject.SetActive(climb);
                interactable.gameObject.SetActive(!climb);
            }
        }
    }
}
