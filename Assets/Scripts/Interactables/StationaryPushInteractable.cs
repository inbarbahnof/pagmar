using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Interactables
{
    public class StationaryPushInteractable : PushInteractable
    {
        [SerializeField] private bool pushRight = false;
        [SerializeField] private float pushDelayTime = 5f;
        [SerializeField] private float rotationTime = 0.5f;
        [SerializeField] private Transform roadblock;

        [Header("Surfaces")]
        [SerializeField] private GameObject _unwalkable;
        [SerializeField] private GameObject _walkable;
        
        private Coroutine _pushDelay;
        private bool _pushing = false;
        private bool _playerMoveDirRight;
        private Quaternion _startRot;

        void Start()
        {
            Stationary = true;
        }
        
        public override void PushObject(float playerX, bool playerMoveDirRight = false)
        {
            _playerMoveDirRight = playerMoveDirRight;
            // start timer
            if (!_pushing)
            {
                _pushDelay = StartCoroutine(PushDelayCoroutine());
            }
        }

        public override void StopInteractPress()
        {
            if (_pushDelay is not null)
            {
                StopCoroutine(_pushDelay);
                _pushDelay = null;
            }

            _pushing = false;
            base.StopInteractPress();
        }

        private IEnumerator PushDelayCoroutine()
        {
            _pushing = true;
            float elapsed = 0f;
            while (elapsed < pushDelayTime)
            {
                if (_playerMoveDirRight != pushRight)
                {
                    elapsed = 0;
                    //Debug.Log("wrong dir");
                }
                elapsed += Time.deltaTime;
                yield return null; // Wait for next frame
            }
            FinishInteraction();
        }

        public override void ResetToCheckpoint()
        {
            roadblock.rotation = _startRot;
            GetComponent<Collider2D>().enabled = true;
        }

        protected override void FinishInteraction()
        {
            if (!_pushing) return;
            Vector3 pushdir = pushRight ? Vector3.right : Vector3.left;
            PushInteractableManager.instance.PushPlayerBack(pushdir);
            StartCoroutine(RotateRoadblockCoroutine());
            _pushing = false;
            base.FinishInteraction();
        }
        
        public IEnumerator RotateRoadblockCoroutine()
        {
            PushInteractableManager.instance.StopPush();
            float elapsed = 0;
            _startRot = roadblock.rotation;
            Quaternion endRot = _startRot * Quaternion.Euler(0, 0, 90); // Rotate 90° around Z-axis
            
            while (elapsed < rotationTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / rotationTime);
                // Smooth interpolation
                roadblock.rotation = Quaternion.Slerp(_startRot, endRot, t);
                yield return null;
            }
            // Ensure final positions are exact
            roadblock.rotation = endRot;
            GetComponent<Collider2D>().enabled = false;
            
            _unwalkable.SetActive(false);
            _walkable.SetActive(true);
            
            NavMeshManager.instance.ReBake();
        }
    }
}