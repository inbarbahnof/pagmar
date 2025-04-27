using System;
using Dog;
using UnityEngine;

namespace Interactables
{
    public class PushInteractableManager : MonoBehaviour
    {
        public static PushInteractableManager instance;
        public event Action OnReachedTarget;

        [SerializeField] private Transform player;
        [SerializeField] private Transform dog;
        
        private PlayerMove _playerMove;
        private DogActionManager _dogAction;

        private PushInteractable _curPushable;
        private float _xOffset;
        private Vector3 _dogOffsetFromObject;
        private bool _isPushing = false;

        private Vector2 _pushTarget;
        
        void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else Debug.LogError("TOO MANY PUSH INTERACTABLE MANAGERS!");

            _playerMove = player.GetComponent<PlayerMove>();
            _dogAction = dog.GetComponent<DogActionManager>();
        }

        public void SetPushTarget(Vector2 target, Action onReachEvent)
        {
            _pushTarget = target;
            OnReachedTarget += onReachEvent;
        }

        public void TryStartPush(PushInteractable interactable)
        {
            _isPushing = true;
            _playerMove.SetIsPushing(true);

            _curPushable = interactable;
            _curPushable.SetOffset(player.position.x);

            if (interactable.NeedDogToPush())
            {
                _dogOffsetFromObject = interactable.transform.position - dog.position;
            }
        }

        public void StopPush()
        {
            _isPushing = false;
            _curPushable = null;
            _playerMove.SetIsPushing(false);
        }

        void Update()
        {
            if (!_isPushing || _curPushable == null) return;
            
            if (_curPushable.NeedDogToPush() && _dogAction.CurState != DogState.Push)
            {
                // print("dog not pushing");
                return;
            }

            _curPushable.PushObject(player.position.x);

            if (_curPushable.NeedDogToPush())
            {
                PushDog();
            }

            if (_pushTarget != Vector2.zero)
            {
                if (Vector2.Distance(_curPushable.transform.position, _pushTarget) < 0.3f)
                {
                    _curPushable.SetAtPos(_pushTarget.x);
                    OnReachedTarget?.Invoke();
                    print("finished");
                    StopPush();
                }
            }
        }
        
        private void PushDog()
        {
            Vector3 dogTargetPos = _curPushable.transform.position - _dogOffsetFromObject;
            dog.position = dogTargetPos;
        }

        public void ResetToCheckpoint(PushInteractable interactable)
        {
            interactable.ResetToCheckpoint();
            StopPush();
        }
        
    }

}
