using System;
using UnityEngine;

namespace Interactables
{
    public class PushInteractable : BaseInteractable
    {
        [SerializeField] protected Transform playerPosToPush;
        [SerializeField] protected bool _isPushingFromLeft = true;
        private SmoothMover _smoothMover;
        
        public enum PushKind
        {
            Wood,
            Trap
        }

        [SerializeField] protected PushKind _pushKind;
        
        private Vector2 _pushTarget;
        public event Action OnReachedTarget;
        private bool hasTarget;
        
        protected bool Stationary = false;
        public bool GetStationary() => Stationary;
        public PushKind pushKind => _pushKind;
        
        public bool IsPushingFromLeft => _isPushingFromLeft;
        
        private float _xOffset;

        public override void Interact()
        {
            if (_smoothMover is null) _smoothMover = GetComponent<SmoothMover>();
            if (hasTarget) PushInteractableManager.instance.SetPushTarget(_pushTarget, OnReachedTarget);
            PushInteractableManager.instance.TryStartPush(this, playerPosToPush.position);
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

        public float SetAtPos(float posX)
        {
            Vector3 newPos = transform.position;
            newPos.x = posX;
            return _smoothMover.MoveTo(newPos);
        }

        public void SetPushTarget(Vector2 target, Action onReachEvent)
        {
            _pushTarget = target;
            OnReachedTarget += onReachEvent;
            hasTarget = true;
        }
    }
}