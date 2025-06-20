using System;
using System.Collections;
using UnityEngine;

namespace Interactables
{
    public class ClimbObject : BaseInteractable
    {
        [SerializeField] private GameObject col;
        [SerializeField] private Transform playerClimbPos;

        public override void Interact()
        {
            Vector2 playerPos = playerClimbPos.position;
            if (playerClimbPos.position == transform.position) playerPos = Vector2.zero;
            ClimbInteractableManager.instance.Climb(this, playerPos);
            col.SetActive(false);
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
                col.SetActive(true);
            }
        }
    }
}
