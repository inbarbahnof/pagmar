using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Targets
{
    public class TargetGenerator : MonoBehaviour
    {
        [SerializeField] private List<Target> targets = new List<Target>();
        [SerializeField] private Target _callTarget;
        private Target _foodTarget;

        public Target GetCallTarget()
        {
            return _callTarget;
        }
        
        public void AddTargets(Target target)
        {
            if (target)
            {
                targets.Add(target);
            }
        }

        public void RemoveTarget(Target target)
        {
            if (target)
            {
                targets.Remove(target);
            }
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
            if (targets.Count == 0) return null;
            Target newTarget = targets[GenerateNewTargetIndex()];
            return newTarget;
        }

        private int GenerateNewTargetIndex()
        {
            return Random.Range(0, targets.Count);
        }

    }
}
