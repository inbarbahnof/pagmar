using System;
using UnityEngine;

namespace Interactabels
{
    public abstract class BaseInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject highlightEffect;
        [SerializeField] protected float interactionRange = 1.5f;
        
        public float InteractionRange => interactionRange;

        // private bool _isInteracting = false;

        /// <summary>
        /// Controlls highlight of interactable obj, turned on when player can interact and off otherwise.
        /// </summary>
        /// <param name="isHighlighted"> Bool val to set highlight activation to </param>
        public void SetHighlight(bool isHighlighted)
        {
            highlightEffect.SetActive(isHighlighted);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                InteractableManager.instance.AddInteractableObj(this);
            }
        }

        /// <summary>
        /// Called when player pressed interact button and this is current interactable obj.
        /// </summary>
        /// <param name="player"> Transform of player interacting with obj </param>
        public virtual void Interact()
        {
        }

        /// <summary>
        /// Returns position for measuring distance from player to find closest interactable.
        /// </summary>
        /// <returns> Position from which to measure dist from player </returns>
        public virtual Vector2 GetCurPos()
        {
            return transform.position;
        }

        /// <summary>
        /// Called when player interacting with obj let go of interact button.
        /// </summary>
        public virtual void StopInteractPress()
        {
            // if done need to finish interaction
            // this may be just picking up an obj and then not done until press again to drop
        }

        /// <summary>
        /// Called when player has stopped interaction with obj, either in the middle of interaction or when finished.
        /// </summary>
        protected void FinishInteraction()
        {
            InteractableManager.instance.OnFinishInteraction();
        }

    }
}