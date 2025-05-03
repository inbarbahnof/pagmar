using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Targets
{
    public class TargetGenerator : MonoBehaviour
    {
        [FormerlySerializedAs("targets")] [SerializeField] private List<Target> playerTargets = new List<Target>();
        [SerializeField] private Target _callTarget;
        private Target _foodTarget;
        
        private List<Target> idleTargets = new List<Target>();

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

        public void AddToIdleTargets(Target target)
        {
            if (target)
            {
                idleTargets.Add(target);
            }
        }
        
        public void RemoveToIdleTargets(Target target)
        {
            if (target)
            {
                idleTargets.Remove(target);
            }
        }
        
        public Target GenerateNewIdleTarget()
        {
            if (idleTargets.Count == 0) return null;
            Target newTarget = playerTargets[Random.Range(0, idleTargets.Count)];
            return newTarget;
        }

        public void SetFoodTarget(Target target)
        {
            _foodTarget = target;
        }

        public Target GetFoodTarget()
        {
            return _foodTarget;
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
