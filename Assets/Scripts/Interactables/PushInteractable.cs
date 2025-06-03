using System;
using UnityEngine;

namespace Interactables
{
    public class PushInteractable : BaseInteractable
    {
        [SerializeField] protected bool _needDogToPush = false;
        [SerializeField] protected Transform playerPosToPush;
        [SerializeField] protected Transform dogPosToPush;
        
        private Vector2 _pushTarget;
        public event Action OnReachedTarget;
        private bool hasTarget;
        
        protected bool Stationary = false;
        public bool GetStationary() => Stationary;
        
        public bool NeedDogToPush() => _needDogToPush;
        
        private float _xOffset;

        public override void Interact()
        {
            if (hasTarget) PushInteractableManager.instance.SetPushTarget(_pushTarget, OnReachedTarget);
            PushInteractableManager.instance.TryStartPush(this, playerPosToPush.position, dogPosToPush.position);
            base.Interact();
        }

        public override void StopInteractPress()
        {
            PushInteractableManager.instance.StopPush();
            base.FinishInteraction();
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

        public void SetPushTarget(Vector2 target, Action onReachEvent)
        {
            _pushTarget = target;
            OnReachedTarget += onReachEvent;
            hasTarget = true;
        }
    }
}