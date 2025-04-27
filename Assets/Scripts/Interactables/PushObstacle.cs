using UnityEngine;
using UnityEngine.Serialization;

namespace Interactables
{
    public class PushObstacle : Obstacle
    {
        [SerializeField] private PushInteractableManager pushManager;
        [SerializeField] private PlayerDeathZone deathZone;
        [SerializeField] private PushInteractable interactable;
        
        private Vector2 _pushTarget;

        void Start()
        {
            _pushTarget = transform.position;
            pushManager.SetPushTarget(_pushTarget, ReachedTarget);
        }

        public override void ReachedTarget()
        {
            base.ReachedTarget();
            if (deathZone != null) deathZone.gameObject.SetActive(false);
        }

        public override void ResetObstacle()
        {
            pushManager.ResetToCheckpoint(interactable);
            if (deathZone != null) deathZone.gameObject.SetActive(true);
        }
        
    }
}
