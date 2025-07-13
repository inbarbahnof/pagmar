using System;
using System.Collections;
using Audio.FMOD;
using Targets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Interactables
{
    public class ThrowablePickUpInteractable : PickUpInteractable
    {
        // [SerializeField] private FoodPickUpInteractable _food;
        [SerializeField] private FoodTarget _foodTarget;
        [SerializeField] private bool _isStealth = true;
        [SerializeField] private ParticleSystem _particle;
        
        private AimControl _aimControl;
        private bool _isThrowing;
        private ThrowInput _curInput;

        public ThrowInput CurThrowInput => _curInput;
        public bool IsThrowing => _isThrowing;
        public event Action<Transform> OnThrowComplete;
        
        public void Throw(ThrowInput input)
        {
            if (_isThrowing) return;

            _curInput = input;
            StartCoroutine(ThrowCoroutine(input));
        }
        
        public override void PickUpObject(Transform parent)
        {
            if (_foodTarget != null) _foodTarget.SetCanBeFed(false);
            base.PickUpObject(parent);
        }

        public override void DropObject(Vector2 worldTarget)
        {
            if (_foodTarget != null)
            {
                ActivateIfOnWalkable(worldTarget);
            }

            base.DropObject(worldTarget);
        }

        private void ActivateIfOnWalkable(Vector2 worldTarget)
        {
            bool isWalkable = IsWalkable(worldTarget);
            
            if (isWalkable)
            {
                _foodTarget.SetCanBeFed(true);
            }
        }
        
        private bool IsWalkable(Vector2 position)
        {
            NavMeshHit hit;
            // You can tweak the maxDistance (0.5f–1f is usually enough)
            bool isWalkable = NavMesh.SamplePosition(position, out hit, 0.5f, NavMesh.AllAreas);
            return isWalkable;
        }
        
        public void ResetState(Vector3 resetPosition, Transform parent)
        {
            StopAllCoroutines();
            transform.position = resetPosition;
            transform.SetParent(parent);
    
            _isThrowing = false;
            typeof(ThrowablePickUpInteractable)
                .GetField("isPickedUp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(this, false);

            GetComponent<Collider2D>().enabled = true;
            gameObject.SetActive(true);
            SetCanInteract(true);
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

            if (_isStealth)
            {
                _particle.Play();
            }
            
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.ObjectFall);
            if (_foodTarget != null) ActivateIfOnWalkable(transform.position);
            
            OnThrowComplete?.Invoke(transform);
        }

        public override void StopInteractPress()
        {
            base.StopInteractPress();
            PickUpInteractableManager.instance.StopInteractPress(this);
        }
    }
}