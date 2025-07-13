using System.Collections;
using Audio.FMOD;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace Interactables
{
    public class PickUpInteractable : BaseInteractable
    {
        [SerializeField] protected bool _isThrowable;
        [SerializeField] protected SortingGroup _sprite;
        [SerializeField] private Animator _shadow;
        
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
            isPickedUp = true;
        }

        public void PhysicallyPickUp(Transform parent)
        {
            transform.SetParent(parent);
            transform.DOLocalMove(Vector3.zero, 0.08f)
                .SetEase(Ease.OutQuad);
            
            // _shadow.SetTrigger("out");
            
            _sprite.sortingOrder = 4;
            
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.PlayerPickUp);
        }

        public virtual void DropObject(Vector2 worldTarget)
        {
            isPickedUp = false;
            _sprite.sortingOrder = 3;
            // _shadow.SetTrigger("in");
            
            if (_isThrowable && worldTarget != Vector2.zero)
            {
                transform.SetParent(originalParent);
                if (worldTarget != Vector2.zero) transform.position = worldTarget;
                FinishInteraction();
            }
            else InteractableManager.instance.FinishPickupInteraction(this, worldTarget, originalParent);
            // else StartCoroutine(FinishAction(worldTarget));
        }

        private IEnumerator FinishAction(Vector2 worldTarget)
        {
            yield return new WaitForSeconds(0.1f);
            
            transform.SetParent(originalParent);
            if (worldTarget != Vector2.zero) transform.position = worldTarget;
            
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.PlayerPickUp);
            FinishInteraction();
        }
    }
}