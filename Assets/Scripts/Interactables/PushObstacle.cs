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
        [SerializeField] private ClimbPushableInteractable firstClimbInter;
        [SerializeField] private bool _isJumping;
        [SerializeField] private DogWaitForPlayer _wait;
        [SerializeField] private ParticleSystem _reachedTargetPartical;
        private Collider2D interCol;
        

        void Start()
        {
            interactable.SetPushTarget(pushTarget.position, ReachedTarget);
            interCol = interactable.GetComponent<Collider2D>();

        }

        public override void ReachedTarget()
        {
            base.ReachedTarget();
            SwapToClimb(true);
            DisableBarrier(true);
            if (_reachedTargetPartical != null) _reachedTargetPartical.Play();
            
            if (_wait != null)
            {
                _wait.PlayerReached();
                pushManager.DogStopCrouching();
            }
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
            interCol.enabled = !enable;
        }

        private void SwapToClimb(bool climb)
        {
            if (climbUpObject)
            {
                climbUpObject.gameObject.SetActive(climb);
                InteractableManager.instance.RemoveInteractable(interactable);
                InteractableManager.instance.AddInteractableObj(firstClimbInter);
                interactable.gameObject.SetActive(!climb);
            }
        }
    }
}
