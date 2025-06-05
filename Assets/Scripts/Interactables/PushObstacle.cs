using UnityEngine;

namespace Interactables
{
    public class PushObstacle : Obstacle
    {
        [SerializeField] private PushInteractableManager pushManager;
        [SerializeField] private PlayerDeathZone deathZone;
        [SerializeField] private PushInteractable interactable;
        [SerializeField] private Transform pushTarget;
        

        void Start()
        {
            interactable.SetPushTarget(pushTarget.position, ReachedTarget);
        }

        public override void ReachedTarget()
        {
            base.ReachedTarget();
            
            if (deathZone != null) deathZone.gameObject.SetActive(false);
            NavMeshManager.instance.ReBake();
        }

        public override void ResetObstacle()
        {
            pushManager.ResetToCheckpoint(interactable);
            
            NavMeshManager.instance.ReBake();
            if (deathZone != null) deathZone.gameObject.SetActive(true);
        }
        
    }
}
