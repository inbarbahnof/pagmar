using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Interactables
{
    public class ThrowablePickUpInteractable : PickUpInteractable
    {
        [SerializeField] private FoodPickUpInteractable _food;
        
        private AimControl _aimControl;
        private bool _isThrowing;

        public bool IsThrowing => _isThrowing;
        public event Action<Transform> OnThrowComplete;
        
        public void Throw(ThrowInput input)
        {
            if (_isThrowing) return;
            StartCoroutine(ThrowCoroutine(input));
        }

        public override void DropObject(Vector2 worldTarget)
        {
            if (_food != null) _food.ActivateIfOnWalkable(worldTarget);
            base.DropObject(worldTarget);
        }

        private IEnumerator ThrowCoroutine(ThrowInput input)
        {
            _isThrowing = true;
            float duration = Vector2.Distance(input.startPoint, input.endPoint) / input.throwSpeed;
            float elapsed = 0f;
        
            // Recalculate control point (identical to OnAim)
            Vector2 controlPoint = (input.startPoint + input.endPoint) * 0.5f;
            controlPoint.y += input.arcHeight;
        
            while (elapsed < duration)
            {
                float t = elapsed / duration;
                Vector2 pos = TrajectoryEvaluator.Evaluate(t, input.startPoint, input.endPoint, controlPoint);
            
                transform.position = pos;
                elapsed += Time.deltaTime;
                yield return null;
            }
        
            transform.position = input.endPoint;
            _isThrowing = false;
            OnThrowComplete?.Invoke(transform);
            
            if (_food != null) _food.ActivateIfOnWalkable(transform.position);
        }

        public override void StopInteractPress()
        {
            base.StopInteractPress();
            PickUpInteractableManager.instance.StopInteractPress(this);
        }
    }
}