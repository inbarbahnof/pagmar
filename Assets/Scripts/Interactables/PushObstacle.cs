using Interactabels;
using Unity.VisualScripting;
using UnityEngine;
using Update = UnityEngine.PlayerLoop.Update;

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
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !_setupComplete)
            {
                pushManager.ResetOnDeath();
            }
        }

        public void ReachedTarget()
        {
            _setupComplete = true;
            GetComponent<Collider2D>().enabled = false;
        }
        
        
        
    }
}
