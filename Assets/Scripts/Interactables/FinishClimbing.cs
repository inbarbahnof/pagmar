using System;
using UnityEngine;

namespace Interactables
{
    class FinishClimbing : MonoBehaviour
    {
        [SerializeField] private bool moveAfterExit = false;
        [SerializeField] private Collider2D rightCol;
        [SerializeField] private Collider2D leftCol;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                ClimbInteractableManager.instance.StopInteraction();
                if (moveAfterExit && leftCol && rightCol)
                {
                    leftCol.enabled = gameObject.transform.localPosition.x <= 0;
                    rightCol.enabled = gameObject.transform.localPosition.x > 0;
                }
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