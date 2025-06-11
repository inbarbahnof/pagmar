using System;
using UnityEngine;

namespace Interactables
{
    class FinishClimbing : MonoBehaviour
    {
        [SerializeField] private ClimbObject _climb;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                ClimbInteractableManager.instance.StopInteraction();
        }
    }
}