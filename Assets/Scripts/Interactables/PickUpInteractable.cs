using System.Collections;
using Audio.FMOD;
using DG.Tweening;
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
            transform.DOLocalMove(Vector3.zero, 0.1f)
                .SetEase(Ease.OutQuad);
            isPickedUp = true;
            
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.PlayerPickUp);
        }

        public virtual void DropObject(Vector2 worldTarget)
        {
            isPickedUp = false;
            transform.SetParent(originalParent);
            if (worldTarget != Vector2.zero) transform.position = worldTarget;
            
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.PlayerPickUp);
            
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