using System;
using System.Collections;
using Audio.FMOD;
using Dog;
using FMOD.Studio;
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
        
        private EventInstance _dragSound;

        private Coroutine moveToTarget;
        
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
            
            if (!_dragSound.isValid())
            {
                _dragSound = AudioManager.Instance.PlayLoopingSound(FMODEvents.Instance.PlayerDrag);
                if (interactable.pushKind == PushInteractable.PushKind.Wood)
                    AudioManager.Instance.SetStringParameter(_dragSound,
                        "Drag Mode", "wood", false);
                else AudioManager.Instance.SetStringParameter(_dragSound,
                    "Drag Mode", "trap", false);
            }
            
            _curPushable = interactable;
            _curPushable.SetOffset(playerPos.x);
            
        }

        public void DogStopCrouching()
        {
            _dogAction.ChangeCrouching(false);
        }

        public void StopPush()
        {
            if (_isPushing)
            {
                _isPushing = false;
                _curPushable.StopInteractPress();
            
                _curPushable = null;
                _playerMove.SetIsPushing(false, Vector3.zero);
            
                AudioManager.Instance.StopSound(_dragSound);
                _dragSound = default;
            }
        }

        void Update()
        {
            if (!_isPushing || _curPushable == null) return;
            
            _curPushable.PushObject(player.position.x, _playerMove.GetMoveDirRight());

            if (_pushTarget != Vector2.zero)
            {
                float targetDist = Vector2.Distance(_curPushable.transform.position, _pushTarget);
                //Debug.Log("targetDist: " + targetDist);
                if (targetDist < 0.3f && moveToTarget == null)
                {
                    moveToTarget = StartCoroutine(ReachedTarget());
                }
            }
        }

        private IEnumerator ReachedTarget()
        {
            float waitTime = _curPushable.SetAtPos(_pushTarget.x);
            StopPush();
            yield return new WaitForSeconds(waitTime);
            OnReachedTarget?.Invoke();
            SetPushTarget(Vector2.zero, null);
            //print("finished");
            moveToTarget = null;
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
