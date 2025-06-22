using System;
using System.Collections;
using UnityEngine;

namespace Interactables
{
    public class ClimbObject : BaseInteractable
    {
        [SerializeField] protected GameObject col;
        [SerializeField] protected bool _isClimbing = true;
        [SerializeField] private bool climbRight = true;

        public override void Interact()
        {
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
            base.FinishInteraction();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                col.SetActive(true);
                
            }
        }
    }
}
