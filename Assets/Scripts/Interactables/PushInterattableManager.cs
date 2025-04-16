using Dog;
using UnityEngine;

namespace Interactabels
{
    public class PushInterattableManager : MonoBehaviour
    {
        public static PushInterattableManager instance;

        [SerializeField] private Transform player;
        [SerializeField] private Transform dog;
        
        private PlayerMove _playerMove;
        private DogActionManager _dogAction;

        private PushInteractable _curPushable;
        private float _xOffset;
        private Vector3 _dogOffsetFromObject;
        private bool _isPushing = false;

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
            if (!_isPushing || _curPushable == null)return;
            
            if (_curPushable.NeedDogToPush() && _dogAction.CurState != DogState.Push)
            {
                print("dog not pushing");
                return;
            }

            _curPushable.PushObject(player.position.x);

            if (_curPushable.NeedDogToPush())
            {
                PushDog();
            }
        }
        
        private void PushDog()
        {
            Vector3 dogTargetPos = _curPushable.transform.position - _dogOffsetFromObject;
            dog.position = dogTargetPos;
        }
    }

}
