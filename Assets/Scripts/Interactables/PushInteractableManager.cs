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
        [SerializeField] private float _pushDistance = 2f;
        
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
            if (dog != null) _dogAction = dog.GetComponent<DogActionManager>();
        }

        public void SetPushTarget(Vector2 target, Action onReachEvent)
        {
            _pushTarget = target;
            // Debug.Log("pushTarget: " + target);
            OnReachedTarget += onReachEvent;
        }

        public void TryStartPush(PushInteractable interactable, Vector3 playerPos)
        {
            _isPushing = true;
            _playerMove.SetIsPushing(true, playerPos, interactable.GetStationary());

            _curPushable = interactable;
            _curPushable.SetOffset(playerPos.x);
            
        }

        public void StopPush()
        {
            _isPushing = false;
            _curPushable = null;
            _playerMove.SetIsPushing(false, Vector3.zero);
        }

        void Update()
        {
            if (!_isPushing || _curPushable == null) return;
            
            _curPushable.PushObject(player.position.x, _playerMove.GetMoveDirRight());

            if (_pushTarget != Vector2.zero)
            {
                float targetDist = Vector2.Distance(_curPushable.transform.position, _pushTarget);
                //Debug.Log("targetDist: " + targetDist);
                if (targetDist < 0.3f)
                {
                    _curPushable.SetAtPos(_pushTarget.x);
                    OnReachedTarget?.Invoke();
                    SetPushTarget(Vector2.zero, null);
                    //print("finished");
                    StopPush();
                }
            }
        }

        public void ResetToCheckpoint(BaseInteractable interactable)
        {
            interactable.ResetToCheckpoint();
            StopPush();
        }

        public void PushPlayerBack(Vector3 pushDir)
        {
            // Player movement setup
            Vector3 startPos = player.position;
            //Vector3 endPos = startPos - pullDirection.normalized * 1f; // 1 unit backward (adjust as needed)
            Vector3 endPos = startPos + pushDir * 1f;
            player.GetComponent<SmoothMover>().MoveTo(endPos);
        }
        
        
        
    }

}
