using System.Collections;
using UnityEngine;

namespace Interactables
{
    public class PickUpInteractable : BaseInteractable
    {
        [SerializeField] protected bool _isThrowable;
        
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

        public virtual void PickUpObject(Transform parent)
        {
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            isPickedUp = true;
        }

        public virtual void DropObject(Vector2 worldTarget)
        {
            isPickedUp = false;
            transform.SetParent(originalParent);
            if (worldTarget != Vector2.zero) transform.position = worldTarget;
            
            if (_isThrowable) FinishInteraction();
            else StartCoroutine(FinishAction());
        }

        private IEnumerator FinishAction()
        {
            yield return new WaitForSeconds(0.1f);
            FinishInteraction();
        }
    }
}