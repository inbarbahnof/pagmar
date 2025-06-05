using System;
using System.Collections.Generic;
using Interactables;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Targets
{
    public class TargetGenerator : MonoBehaviour
    {
        public static TargetGenerator instance;
        
        [FormerlySerializedAs("targets")] [SerializeField] private List<Target> playerTargets = new List<Target>();
        [SerializeField] private Target _callTarget;
        
        private FoodTarget _foodTarget;
        private Target _stickTarget;
        private WantFoodTarget _wantFoodTarget;
        private Target _stealthTarget;

        private bool _didStealthTargetChange;
        
        // private List<Target> idleTargets = new List<Target>();

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else Debug.LogError("TOO MANY TARGET GENERATORS!");
        }

        public Target GetCallTarget()
        {
            return _callTarget;
        }
        
        public void AddToPlayerTargets(Target target)
        {
            if (target)
            {
                playerTargets.Add(target);
            }
        }
        
        public void RemoveFromPlayerTarget(Target target)
        {
            if (target)
            {
                playerTargets.Remove(target);
            }
        }

        public bool DidStealthTargetChange()
        {
            return _didStealthTargetChange && _stealthTarget != null;
        }
        
        public void SetStealthTarget(Target target)
        {
            _didStealthTargetChange = true;
            _stealthTarget = target;
            // print("stealth target set to " + target);
        }

        public Target GetStealthTarget(bool toFollow)
        {
            if (toFollow) _didStealthTargetChange = false;
            return _stealthTarget;
        }
        
        public void SetStickTarget(Target target)
        {
            _stickTarget = target;
        }

        public Target GetStickTarget()
        {
            return _stickTarget;
        }
        
        public void SetWantFoodTarget(WantFoodTarget target)
        {
            _wantFoodTarget = target;
        }

        public WantFoodTarget GetWantFoodTarget()
        {
            return _wantFoodTarget;
        }

        public void SetFoodTarget(FoodTarget target)
        {
            _foodTarget = target;
        }
        
        public void NotifyFoodNearby(FoodTarget food)
        {
            if (food != null && !food.GetComponent<PickUpInteractable>().IsPickedUp && food.CanBeFed)
                _foodTarget = food;
        }

        public void NotifyFoodFar(FoodTarget food)
        {
            if (_foodTarget == food)
                _foodTarget = null;
        }

        public FoodTarget GetFoodTarget()
        {
            if (_foodTarget != null && _foodTarget.CanBeFed)
                return _foodTarget;
                
            return null;
        }

        public Target GenerateNewTarget()
        {
            if (playerTargets.Count == 0) return null;
            Target newTarget = playerTargets[GenerateNewTargetIndex()];
            return newTarget;
        }

        private int GenerateNewTargetIndex()
        {
            return Random.Range(0, playerTargets.Count);
        }

    }
}

// public void AddToIdleTargets(Target target)
// {
//     if (target)
//     {
//         idleTargets.Add(target);
//     }
// }
//         
// public void RemoveToIdleTargets(Target target)
// {
//     if (target)
//     {
//         idleTargets.Remove(target);
//     }
// }
//         
// public Target GenerateNewIdleTarget()
// {
//     if (idleTargets.Count == 0) return null;
//     Target newTarget = playerTargets[Random.Range(0, idleTargets.Count)];
//     return newTarget;
// }