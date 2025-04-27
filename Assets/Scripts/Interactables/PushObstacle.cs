using UnityEngine;

namespace Interactables
{
    public class PushObstacle : Obstacle
    {
        [SerializeField] private PushInteractableManager pushManager;
        private Vector2 _pushTarget;

        void Start()
        {
            _pushTarget = transform.position;
            pushManager.SetPushTarget(_pushTarget, ReachedTarget);
        }

        public override void ReachedTarget()
        {
            base.ReachedTarget();
            var death = GetComponentInChildren<PlayerDeathZone>();
            if (death != null) death.TrunOff();
        }
        
        
        
    }
}
