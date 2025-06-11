using System;
using System.Collections;
using UnityEngine;

namespace Interactables
{
    public class ClimbObject : BaseInteractable
    {
        [SerializeField] private GameObject _collider;

        public override void Interact()
        {
            ClimbInteractableManager.instance.Climb(this);
            _collider.SetActive(false);
        }

        public override void StopInteractPress()
        {
            
        }

        public void StopInteraction()
        {
            base.FinishInteraction();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _collider.SetActive(true);
                
            }
        }
    }
}
