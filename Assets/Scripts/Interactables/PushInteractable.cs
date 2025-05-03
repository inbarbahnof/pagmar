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

        private Vector3 ogPos;

        private void Start()
        {
            ogPos = transform.position;
        }

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

        public override void ResetToCheckpoint()
        {
            transform.position = ogPos;
        }
    }
}


// private Transform _player;
// private Transform _dog;
// private PlayerMove _playerMove;
// private bool _isBeingPushed = false;
        
// private DogActionManager _dogAction;
// private Vector3 _dogOffsetFromObject;

// [SerializeField] private float pushSpeed = 5f;
// public override void Interact()
// {
// base.Interact(player, dog);
//
// _player = player;
// _isBeingPushed = true;
//
// _playerMove = _player.GetComponent<PlayerMove>();
// _playerMove.SetIsPushing(true);
//
// _xOffset = transform.position.x - player.position.x;
//
// if (_needDogToPush)
// {
//     _dog = dog;
//     _dogAction = dog.GetComponent<DogActionManager>();
//     _dogOffsetFromObject = transform.position - dog.position;
// }
// }
// public override void StopInteractPress()
// {
//     _isBeingPushed = false;
//     _player = null;
//
//     _playerMove.SetIsPushing(false);
//     _playerMove = null;
//
//     _dog = null;
//     _dogAction = null;
//
//     FinishInteraction();
// }

// private void Update()
// {
//     if (_isBeingPushed && _player != null)
//     {
//         if (_needDogToPush)
//         {
//             if (_dogAction.CurState == DogState.Push)
//             {
//                 PushObject();
//                 PushDog();
//             }
//         }
//         else
//         {
//             PushObject();
//         }
//     }
// }