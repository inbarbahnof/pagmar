using System;
using System.Collections;
using UnityEngine;

namespace Interactabels
{
    public class PickUpInteractable : BaseInteractable
    {
        private bool isPickedUp = false;
        private Transform originalParent;
        
        public bool IsPickedUp => isPickedUp;

        private void Start()
        {
            originalParent = transform.parent;
        }

        public override void Interact()
        {
            PickUpInteractableManager.instance.Interact(this);
        }

        public void PickUpObject(Transform parent)
        {
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            isPickedUp = true;
        }

        public void DropObject(Vector2 worldTarget)
        {
            isPickedUp = false;
            transform.SetParent(originalParent);
            if (worldTarget != Vector2.zero) transform.position = worldTarget;
            StartCoroutine(FinishAction());
        }

        private IEnumerator FinishAction()
        {
            yield return new WaitForSeconds(0.1f);
            FinishInteraction();
        }
    }
}