using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = System.Numerics.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Interactables
{
    public class PushInteractable : BaseInteractable
    {
        [SerializeField] private bool _needDogToPush = false;
        [SerializeField] private Transform playerPosToPush;
        [SerializeField] private Transform dogPosToPush;
        
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

        public void PushObject(float playerX)
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