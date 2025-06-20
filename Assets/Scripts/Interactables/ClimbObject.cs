using System;
using System.Collections;
using UnityEngine;

namespace Interactables
{
    public class ClimbObject : BaseInteractable
    {
        [SerializeField] private GameObject col;
        [SerializeField] private Transform playerClimbPos;
        [SerializeField] private bool _isClimbing = true;

        public override void Interact()
        {
            Vector2 playerPos = playerClimbPos.position;
            if (playerClimbPos.position == transform.position) playerPos = Vector2.zero;
            ClimbInteractableManager.instance.Climb(this, playerPos, _isClimbing);
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
            ClimbInteractableManager.instance.StopInteraction();
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
