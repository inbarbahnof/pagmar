using System;
using UnityEngine;

namespace Interactables
{
    class FinishClimbing : MonoBehaviour
    {
        [SerializeField] private bool moveAfterExit = false;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                ClimbInteractableManager.instance.StopInteraction();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player") && moveAfterExit)
            {
                Vector2 newPos = gameObject.transform.localPosition;
                newPos.x = -gameObject.transform.localPosition.x;
                gameObject.transform.localPosition = newPos;
            }
        }
    }
}