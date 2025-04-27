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

        private SwingInteractable _interactable;
        
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
            if (_interactable != null)
            {
                print("Already interacting with a swing!");
                return;
            }
            _interactable = interactable;
            AttachPlayerToRope();
            interactable.StartSwing();
        }
        
        private void AttachPlayerToRope()
        {
            _playerMove.SetIsSwinging(true);
            _ogPlayerRot = player.rotation;
            _ogPlayerYPos = player.position.y;
            player.SetParent(_interactable.AttachLoc);
            UpdatePlayerPos();
            OnSwingStart?.Invoke();
        }

        private void UpdatePlayerPos()
        {
            player.position = _interactable.AttachLoc.position;
            player.rotation = _interactable.AttachLoc.rotation;
        }

        public void FinishSwing(SwingInteractable interactable)
        {
            if (_interactable != interactable)
            {
                print("Trying to detach from wrong interactable obj");
                return;
            }
            DetachAndReset();
        }

        public void StopSwing(SwingInteractable interactable)
        {
            if (_interactable != interactable)
            {
                print("Trying to detach from wrong interactable obj");
                return;
            }
            DetachAndReset();
        }

        private void DetachAndReset()
        {
            _playerMove.SetIsSwinging(false);
            player.SetParent(null);
            player.rotation = _ogPlayerRot;
            player.position = new Vector2(player.position.x, _ogPlayerYPos);
            _interactable.ResetSwing();
            _interactable = null;
            OnSwingStop?.Invoke();
        }
    }
}