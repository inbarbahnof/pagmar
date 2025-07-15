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
        protected bool _currentlyInteractable = true;
        protected Coroutine _waitToStopInteractCoroutine;
        
        public Collider2D GetColGO()
        {
            return interTrigger;
        }
        
        public override void Interact()
        {
            bool playerDirRight = ClimbInteractableManager.instance.GetPlayerMovingRight();
            if (playerDirRight != climbRight || !interTrigger.enabled || !_currentlyInteractable)
            {
                InteractableManager.instance.OnFinishInteraction();
                return;
            }
            //print("player right: " + playerDirRight + " climb right: " + climbRight);
            //print("interacting: " + name);
            _currentlyInteractable = false;
            
            if (twinTrigger is null && twin is not null) twinTrigger = twin.GetColGO();
            ClimbInteractableManager.instance.Climb(this, Vector2.zero, _isClimbing, climbRight);
            if (col)
            {
                col.SetActive(false);
            }
            base.Interact();
        }

        public override void StopInteractPress()
        {
            if (_currentlyInteractable || _waitToStopInteractCoroutine is not null) return;
            _waitToStopInteractCoroutine = StartCoroutine(WaitToStopInteract());
        }

        private IEnumerator WaitToStopInteract()
        {
            float time;
            if (_isClimbing) time = 2.1f;
            else time = 0.8f;
            
            yield return new WaitForSeconds(time); 
            FinishInteraction();
            _waitToStopInteractCoroutine = null;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (col)
                {
                    col.SetActive(true);
                }
                
            }
        }

        public override void FinishInteraction()
        {
            //Debug.Log("finish " + name);
            if (twinTrigger && interTrigger.enabled)
            {
                twinTrigger.enabled = true;
                interTrigger.enabled = false;
                //Debug.Log("col deactivated at finish interact");
            }
            InteractableManager.instance.OnFinishInteraction(twin);
            StartCoroutine(CoolDown());
            if (col)
            {
                col.SetActive(true);
            }
        }

        private IEnumerator CoolDown()
        {
            yield return new WaitForSeconds(1f);
            _currentlyInteractable = true;
        }
    }
}
