﻿using UnityEngine;

namespace Interactables
{
    public abstract class BaseInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] protected GameObject highlightEffect;
        [SerializeField] protected float interactionRange = 1.5f;
        [SerializeField] protected TextAppear textPrompt;
        [SerializeField] protected SpriteFade _highlightSpriteFade;

        protected bool _canInteract = true;
        protected Vector3 ogPos;
        
        public bool CanInteract => _canInteract;
        public float InteractionRange => interactionRange;

        private void Start()
        {
            ogPos = transform.position;
        }
        

        public void SetCanInteract(bool can)
        {
            _canInteract = can;
        }

        /// <summary>
        /// Controlls highlight of interactable obj, turned on when player can interact and off otherwise.
        /// </summary>
        /// <param name="isHighlighted"> Bool val to set highlight activation to </param>
        public virtual void SetHighlight(bool isHighlighted)
        {
            if (highlightEffect is null) return;
            if (_highlightSpriteFade)
            {
                if (!highlightEffect.activeInHierarchy) highlightEffect.SetActive(true);
                _highlightSpriteFade.FadeOutOverTime(isHighlighted);
            }
            else highlightEffect.SetActive(isHighlighted);
            //Debug.Log(highlightEffect.activeInHierarchy);
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
            if (textPrompt) textPrompt.StopShowText();
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

        public virtual void ResetToCheckpoint()
        {
            if (textPrompt) textPrompt.StopShowText();
            transform.position = ogPos;
        }

        /// <summary>
        /// Called when player has stopped interaction with obj, either in the middle of interaction or when finished.
        /// </summary>
        public virtual void FinishInteraction()
        {
            InteractableManager.instance.OnFinishInteraction();
        }
        

    }
}