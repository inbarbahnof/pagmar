using System;
using Targets;
using UnityEngine;

namespace Interactables
{
    public class PickUpInteractableManager : MonoBehaviour
    {
        public static PickUpInteractableManager instance;
        
        public event Action OnReachedTarget;
        public event Action OnPickedUp;

        [SerializeField] private GameObject player;
        [SerializeField] private AimControl aimControl;

        private Transform _playerTransform;
        private PlayerStateManager _playerStateManager;

        private Transform _pickUpParent;
        private Vector2 _carryTarget = Vector2.zero;

        private bool _isAbleToAim;
        public bool IsAbleToAim => _isAbleToAim;
        
        void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else Debug.LogError("TOO MANY PICKUP INTERACTABLE MANAGERS!");
            
            _pickUpParent = player.GetComponent<PickUpParentManager>().GetPickUpParent();
            _playerStateManager = player.GetComponent<PlayerStateManager>();
        }
        
        // TODO check about this being set on start, maybe not supposed
        public void SetCarryTarget(Vector2 target, Action onReachEvent, Action onPickedUp)
        {
            _carryTarget = target;
            OnReachedTarget += onReachEvent;
            OnPickedUp += onPickedUp;
        }
        
        public void Interact(PickUpInteractable pickup)
        {
            if (!pickup.IsPickedUp)
            {
                pickup.PickUpObject(_pickUpParent);
                OnPickedUp?.Invoke();
            }
            else
            {
                // if throwable -> can aim
                if (pickup is ThrowablePickUpInteractable)
                {
                    _isAbleToAim = true;
                    _playerStateManager.UpdateAimAbility(true);
                    _playerStateManager.SetState(PlayerState.Aim);
                }
                else if (Vector2.Distance(pickup.transform.position, _carryTarget) <= 0.5f)
                {
                    pickup.DropObject(_carryTarget);
                    OnReachedTarget?.Invoke();
                }
                else
                {
                    pickup.DropObject(Vector2.zero);
                }
                SetCarryTarget(Vector2.zero, null, null);
            }
        }
        
        public void StopInteractPress(ThrowablePickUpInteractable throwableObj)
        {
            if (!throwableObj.IsPickedUp || !_isAbleToAim) return;
            if (aimControl.IsAiming)
            {
                throwableObj.Throw(aimControl.GetCurThrowInput());
                TargetGenerator.instance.SetStickTarget(throwableObj.GetComponent<Target>());
                _playerStateManager.SetState(PlayerState.Throw);
            }
            throwableObj.DropObject(Vector2.zero);
            _isAbleToAim = false;
            _playerStateManager.UpdateAimAbility();
            _playerStateManager.SetState(PlayerState.Idle);
            aimControl.HideTrajectory();
        }
    }
}