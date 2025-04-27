using System;
using UnityEngine;

namespace Interactables
{
    public class SwingInteractableManager : MonoBehaviour
    {
        public static SwingInteractableManager instance;
        public event Action OnSwingStart;
        public event Action OnSwingStop;

        [SerializeField] private Transform player;
        private PlayerMove _playerMove;
        
        private Quaternion _ogPlayerRot;
        private float _ogPlayerYPos;

        private SwingInteractable _curInteractable;
        private SwingInteractable _prevInteractable;
        
        void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else Debug.LogError("TOO MANY PUSH INTERACTABLE MANAGERS!");

            _playerMove = player.GetComponent<PlayerMove>();

        }

        public void SetSwingEndAction(Action startAction, Action stopAction)
        {
            OnSwingStart += startAction;
            OnSwingStop += stopAction;
        }

        public void TryStartSwing(SwingInteractable interactable)
        {
            if (_curInteractable != null)
            {
                print("Already interacting with a swing!");
                return;
            }
            _curInteractable = interactable;
            AttachPlayerToRope();
            interactable.StartSwing();
        }
        
        private void AttachPlayerToRope()
        {
            _playerMove.SetIsSwinging(true);
            _ogPlayerRot = player.rotation;
            _ogPlayerYPos = player.position.y;
            player.SetParent(_curInteractable.AttachLoc);
            UpdatePlayerPos();
            OnSwingStart?.Invoke();
        }

        private void UpdatePlayerPos()
        {
            player.position = _curInteractable.AttachLoc.position;
            player.rotation = _curInteractable.AttachLoc.rotation;
        }

        public void FinishSwing(SwingInteractable interactable)
        {
            if (_curInteractable is not null && _curInteractable != interactable)
            {
                print("Trying to detach from wrong interactable obj");
                return;
            }
            OnSwingStop?.Invoke();
            ResetSwing();
            DetachPlayer(true);
        }

        public void StopSwing(SwingInteractable interactable)
        {
            if (_curInteractable is null) return;
            if (_curInteractable != interactable)
            {
                print("Trying to detach from wrong interactable obj");
                return;
            }
            OnSwingStop?.Invoke();
            ResetSwing();
            DetachPlayer(true);
        }

        private void ResetSwing()
        {
            if (_curInteractable is null) return;
            _curInteractable.ResetSwing();
            _prevInteractable = _curInteractable;
            _curInteractable = null;
        }

        private void DetachPlayer(bool reposition = false)
        {
            _playerMove.SetIsSwinging(false);
            player.SetParent(null);
            player.rotation = _ogPlayerRot;
            if (reposition) player.position = new Vector2(player.position.x, _ogPlayerYPos);
        }

        public void ResetToCheckpoint(SwingInteractable interactable)
        {
            interactable.ResetToCheckpoint();
            DetachPlayer();
        }
    }
}