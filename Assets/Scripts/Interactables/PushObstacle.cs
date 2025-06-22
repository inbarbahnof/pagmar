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
        [SerializeField] private GameObject climbDownObject;
        [SerializeField] private bool _isJumping;
        

        void Start()
        {
            interactable.SetPushTarget(pushTarget.position, ReachedTarget);
        }

        public override void ReachedTarget()
        {
            base.ReachedTarget();
            SwapToClimb(true);
        }

        public override void ResetObstacle()
        {
            pushManager.ResetToCheckpoint(interactable);
            SwapToClimb(false);
        }

        private void SwapToClimb(bool climb)
        {
            if (colMid != null) colMid.SetActive(!climb);
            if (climbUpObject)
            {
                climbUpObject.SetActive(climb);
                interactable.gameObject.SetActive(!climb);
            }
            if (!_isJumping) NavMeshManager.instance.ReBake();
        }
    }
}
