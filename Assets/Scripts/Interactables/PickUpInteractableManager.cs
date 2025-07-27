using System;
using Spine;
using Spine.Unity;
using Targets;
using UnityEngine;

namespace Interactables
{
    public class PickUpInteractableManager : MonoBehaviour
    {
        public static PickUpInteractableManager instance;
        
        public event Action OnReachedTarget;
        public event Action OnPickedUp;
        
        [SerializeField] SkeletonAnimation skeletonAnimation;
        [SerializeField] private GameObject player;
        [SerializeField] private AimControl aimControl;

        private Transform _playerTransform;
        private PlayerStateManager _playerStateManager;

        private Transform _pickUpParent;
        private Vector2 _carryTarget = Vector2.zero;

        private bool _isAbleToAim;
        private PickUpInteractable _curPickUp;
        public PickUpInteractable CurPickUp => _curPickUp;
        
        private Spine.AnimationState spineAnimationState;
        private Spine.Skeleton skeleton;
        
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

        public void PickUpCurrentObject()
        {
            if (_curPickUp != null && _curPickUp.IsPickedUp) _curPickUp.PhysicallyPickUp(_pickUpParent);
        }
        
        public void Interact(PickUpInteractable pickup)
        {
            if (!pickup.IsPickedUp)
            {
                _curPickUp = pickup;
                _curPickUp.PickUpObject(_pickUpParent);
                _playerStateManager.UpdatePickedUp(true);
                OnPickedUp?.Invoke();
            }
            else
            {
                // if throwable -> can aim
                if (pickup is ThrowablePickUpInteractable)
                {
                    _isAbleToAim = true;
                    _playerStateManager.UpdateAimAbility(true);
                    _playerStateManager.UpdateThrowState(PlayerStateManager.ThrowState.Aim);
                }
                else if (Vector2.Distance(pickup.transform.position, _carryTarget) <= 0.5f)
                {
                    pickup.DropObject(_carryTarget);
                    _playerStateManager.UpdatePickedUp(false);
                    _curPickUp = null;
                    OnReachedTarget?.Invoke();
                }
                else
                {
                    pickup.DropObject(Vector2.zero);
                    _playerStateManager.UpdatePickedUp(false);
                    _curPickUp = null;
                }
                SetCarryTarget(Vector2.zero, null, null);
            }
        }

        public void DropObject(Vector2 pos)
        {
            if (_curPickUp != null) _curPickUp.DropObject(pos);
        }
        
        public void StopInteractPress(ThrowablePickUpInteractable throwableObj)
        {
            if (!throwableObj.IsPickedUp || !_isAbleToAim) return;
            if (aimControl.IsAiming && aimControl.CurAimValid)
            {
                throwableObj.Throw(aimControl.GetCurThrowInput());
                TargetGenerator.instance.SetStickTarget(throwableObj.GetComponent<Target>());
                _playerStateManager.UpdateThrowState(PlayerStateManager.ThrowState.Throw);
            }
            throwableObj.DropObject(Vector2.zero);
            _isAbleToAim = false;
            _playerStateManager.UpdateAimAbility();
            _playerStateManager.UpdateThrowState(PlayerStateManager.ThrowState.End);
            aimControl.HideTrajectory();
        }
    }
}