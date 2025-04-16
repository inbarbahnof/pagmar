using System;
using Dog;
using UnityEngine;

namespace Interactabels
{
    public class PushInteractable : BaseInteractable
    {
        [SerializeField] private bool _needDogToPush = false;
        public bool NeedDogToPush() => _needDogToPush;
        
        private float _xOffset;
        
        public override void Interact()
        {
            PushInterattableManager.instance.TryStartPush(this);
            base.Interact();
        }

        public override void StopInteractPress()
        {
            PushInterattableManager.instance.StopPush();
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