using System;
using System.Collections;
using Targets;
using UnityEngine;
using UnityEngine.AI;

namespace Interactables
{
    public class FoodPickUpInteractable : PickUpInteractable
    {
        public event Action<FoodPickUpInteractable> OnDroppedOnWalkableSurface;
        // [SerializeField] private SpriteRenderer _food;
        [SerializeField] private FoodTarget _foodTarget;

        public override void PickUpObject(Transform parent)
        {
            _foodTarget.SetCanBeFed(false);
            base.PickUpObject(parent);
        }

        public override void DropObject(Vector2 worldTarget)
        {
            base.DropObject(worldTarget);
            _foodTarget.SetCanBeFed(true);
            ActivateIfOnWalkable(worldTarget);
        }

        public void ActivateIfOnWalkable(Vector2 worldTarget)
        {
            bool isWalkable = IsWalkable(worldTarget);
            
            if (isWalkable)
            {
                StartCoroutine(WaitToTransformScale());
                _foodTarget.SetCanBeFed(true);
            }
        }

        private IEnumerator WaitToTransformScale()
        {
            yield return new WaitForSeconds(0.1f);
            if (IsWalkable(transform.position)) OnDroppedOnWalkableSurface?.Invoke(this);
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }

        public void FoodCanBeFed()
        {
            _foodTarget.SetCanBeFed(true);
        }
        
        private bool IsWalkable(Vector2 position)
        {
            NavMeshHit hit;
            // You can tweak the maxDistance (0.5f–1f is usually enough)
            bool isWalkable = NavMesh.SamplePosition(position, out hit, 0.5f, NavMesh.AllAreas);
            return isWalkable;
        }
    }
}