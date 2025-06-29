using System.Collections;
using UnityEngine;

namespace Interactables
{
    public class ClimbObject : BaseInteractable
    {
        [SerializeField] protected GameObject col;
        [SerializeField] protected Collider2D interTrigger;
        [SerializeField] protected bool _isClimbing = true;
        [SerializeField] protected bool climbRight = true;
        [SerializeField] protected ClimbObject twin;
        protected Collider2D twinTrigger;
        
        public Collider2D GetColGO()
        {
            return interTrigger;
        }
        
        public override void Interact()
        {
            if (twinTrigger is null && twin is not null) twinTrigger = twin.GetColGO();
            ClimbInteractableManager.instance.Climb(this, Vector2.zero, _isClimbing, climbRight);
            col.SetActive(false);
        }

        public override void StopInteractPress()
        {
            StartCoroutine(WaitToStopInteract());
        }

        private IEnumerator WaitToStopInteract()
        {
            float time;
            if (_isClimbing) time = 2.1f;
            else time = 0.8f;
            
            yield return new WaitForSeconds(time); 
            FinishInteraction();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                col.SetActive(true);
                
            }
        }

        public override void FinishInteraction()
        {
            if (twinTrigger)
            {
                twinTrigger.enabled = true;
                interTrigger.enabled = false;
            }
            InteractableManager.instance.OnFinishInteraction(twin);
        }
    }
}
