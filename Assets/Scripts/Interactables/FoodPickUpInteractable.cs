using System;
using Targets;
using UnityEngine;
using UnityEngine.AI;

namespace Interactables
{
    public class FoodPickUpInteractable : PickUpInteractable
    {
        public event Action<FoodPickUpInteractable> OnDroppedOnWalkableSurface;
        
        private FoodTarget _foodTarget;

        private void Awake()
        {
            _foodTarget = GetComponent<FoodTarget>();
        }

        public override void DropObject(Vector2 worldTarget)
        {
            base.DropObject(worldTarget);
            
            bool isWalkable = IsWalkable(worldTarget);

            if (isWalkable)
            {
                _foodTarget.SetCanBeFed(true);
                OnDroppedOnWalkableSurface?.Invoke(this);
            }
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