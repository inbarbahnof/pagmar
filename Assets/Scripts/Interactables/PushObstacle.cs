using Interactabels;
using Unity.VisualScripting;
using UnityEngine;
using Update = UnityEngine.PlayerLoop.Update;

namespace Interactables
{
    public class PushObstacle : Obstacle
    {
        private PushInteractableManager _pushManager;
        private bool _setupComplete;
        private Vector2 _pushTarget;

        void Start()
        {
            _pushTarget = transform.position;
            _pushManager.SetPushTarget(_pushTarget);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !_setupComplete)
            {
                _pushManager.ResetOnDeath();
            }
        }

        public void ReachedTarget()
        {
            _setupComplete = true;
            // TODO called on event
        }
        
        
        
    }
}
