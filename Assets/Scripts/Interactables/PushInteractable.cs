using System;
using UnityEngine;

namespace Interactables
{
    public class PushInteractable : BaseInteractable
    {
        [SerializeField] protected Transform playerPosToPush;
        [SerializeField] protected bool _isPushingFromLeft = true;
        [SerializeField] private Transform _pushParticals;
        private SmoothMover _smoothMover;
        private Vector3 _particalOffsetFromPush;
        
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
        
        private void Start()
        {
            ogPos = transform.position;
            if (_pushParticals != null) _particalOffsetFromPush = _pushParticals.position - transform.position;
            if (highlightEffect) _highlightSpriteFade = highlightEffect.GetComponent<SpriteFade>();
        }
        
        public override void Interact()
        {
            if (_smoothMover is null) _smoothMover = GetComponent<SmoothMover>();
            if (hasTarget) PushInteractableManager.instance.SetPushTarget(_pushTarget, OnReachedTarget);
            StartCoroutine(PushInteractableManager.instance.TryStartPush(this, playerPosToPush.position));
            base.Interact();
        }

        public override void StopInteractPress()
        {
            PushInteractableManager.instance.StopPush();
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
            
            if (_pushParticals != null)
            {
                _pushParticals.position = transform.position + _particalOffsetFromPush;
            }
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