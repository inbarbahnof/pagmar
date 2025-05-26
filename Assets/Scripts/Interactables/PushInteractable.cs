using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Interactables
{
    public class PushInteractable : BaseInteractable
    {
        [SerializeField] protected bool _needDogToPush = false;
        [SerializeField] protected Transform playerPosToPush;
        [SerializeField] protected Transform dogPosToPush;
        
        protected bool Stationary = false;
        public bool GetStationary() => Stationary;
        
        public bool NeedDogToPush() => _needDogToPush;
        
        private float _xOffset;

        public override void Interact()
        {
            PushInteractableManager.instance.TryStartPush(this, playerPosToPush.position, dogPosToPush.position);
            base.Interact();
        }

        public override void StopInteractPress()
        {
            PushInteractableManager.instance.StopPush();
            FinishInteraction();
        }
        
        public void SetOffset(float playerX)
        {
            _xOffset = transform.position.x - playerX;
        }

        public virtual void PushObject(float playerX, bool playerMoveDirRight = false)
        {
            Vector3 newPos = transform.position;
            newPos.x = playerX + _xOffset;
            transform.position = newPos;
        }

        public void SetAtPos(float posX)
        {
            Vector3 newPos = transform.position;
            newPos.x = posX;
            transform.position = newPos;
        }
    }
}