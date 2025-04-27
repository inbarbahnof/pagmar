using UnityEngine;

namespace Interactables
{
    public class PushObstacle : Obstacle
    {
        [SerializeField] private PushInteractableManager pushManager;
        private bool _setupComplete;
        private Vector2 _pushTarget;

        void Start()
        {
            _pushTarget = transform.position;
            pushManager.SetPushTarget(_pushTarget, ReachedTarget);
        }

        public void ReachedTarget()
        {
            _setupComplete = true;
            var death = GetComponentInChildren<PlayerDeathZone>();
            if (death != null) death.TrunOff();
        }
        
        
        
    }
}
