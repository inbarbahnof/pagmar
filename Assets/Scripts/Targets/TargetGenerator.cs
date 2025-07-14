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
        
        [FormerlySerializedAs("targets")] 
        [SerializeField] private List<Target> playerTargets = new List<Target>();
        [SerializeField] private Target _callTarget;
        [SerializeField] private Target _petTarget;
        [SerializeField] private List<Target> targetsOfInterest = new List<Target>();
        
        private float _TOI_DetectionRadius = 10f;
        private FoodTarget _foodTarget;
        private Target _stickTarget;
        private WantFoodTarget _wantFoodTarget;
        private Target _stealthTarget;
        private List<Target> _ghostiesTargets = new List<Target>();

        private bool _didStealthTargetChange;

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

        public Target GetPetTarget()
        {
            return _petTarget;
        }
        
        public void SetStealthTarget(Target target)
        {
            _didStealthTargetChange = true;
            _stealthTarget = target;
            // print("stealth target set to " + target);
        }

        public Target GetStealthTarget(bool toFollow)
        {
            if (toFollow && _stealthTarget != _callTarget) _didStealthTargetChange = false;
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

        public void AddToGhostiesTargets(Target ghostie)
        {
            if (!_ghostiesTargets.Contains(ghostie))
            {
                _ghostiesTargets.Add(ghostie);
            }
        }

        public void RemoveFromGhostiesTargets(Target target)
        {
            _ghostiesTargets.Remove(target);
        }

        public bool IsThereGhositeClose()
        {
            return _ghostiesTargets.Count > 0;
        }

        public Target GetClosestGhostie(Transform dog)
        {
            if (_ghostiesTargets == null || _ghostiesTargets.Count == 0 || dog == null)
                return null;

            Target closest = null;
            float minDistance = float.MaxValue;

            foreach (Target ghost in _ghostiesTargets)
            {
                if (ghost == null) continue;

                float dist = Vector3.Distance(ghost.transform.position, dog.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = ghost;
                }
            }

            return closest;
        }
        
        public Target GetNearbyTOI(Transform dogTransform)
        {
            if (dogTransform == null || targetsOfInterest.Count == 0)
                return null;

            float minDistance = float.MaxValue;
            Target closest = null;

            foreach (var target in targetsOfInterest)
            {
                if (target == null) continue;

                float dist = Vector2.Distance(target.transform.position, dogTransform.position);
                
                if (dist <= _TOI_DetectionRadius && dist < minDistance && dist > 1)
                {
                    minDistance = dist;
                    closest = target;
                }
            }
            // print("closest " + closest.name);
            return closest;
        }

        public void SetFoodTarget(FoodTarget target)
        {
            _foodTarget = target;
        }
        
        public void NotifyFoodNearby(FoodTarget food)
        {
            PickUpInteractable pickUp = food.GetPickup();

            if (pickUp != null)
            {
                if (food != null && !pickUp.IsPickedUp && food.CanBeFed)
                    _foodTarget = food;
            }
            else if (food.CanBeFed)
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
            {
                return _foodTarget;
            }
                
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