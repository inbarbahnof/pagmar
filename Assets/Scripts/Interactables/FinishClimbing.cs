using System;
using UnityEngine;

namespace Interactables
{
    class FinishClimbing : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                ClimbInteractableManager.instance.StopInteraction();
        }
    }
}