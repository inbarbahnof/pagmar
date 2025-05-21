using System;
using UnityEngine;
using Targets;

namespace Dog
{
    public class DogTargetDetector : MonoBehaviour
    {
        [SerializeField] private DogActionManager _actionManager;
        [SerializeField] private TargetGenerator _targetGenerator;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Food"))
            {
                print("food is close to detector");
                _actionManager.FoodIsClose(other);
            }
            // else
            // {
            //     Target potentialTarget = other.GetComponent<Target>();
            //     if (potentialTarget != null && potentialTarget.IsTOI)
            //     {
            //         _targetGenerator.AddToIdleTargets(potentialTarget);
            //     }
            // }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Food"))
            {
                _actionManager.FoodIsFar(other);
            }
            // else
            // {
            //     Target potentialTarget = other.GetComponent<Target>();
            //     if (potentialTarget != null && potentialTarget.IsTOI)
            //     {
            //         _targetGenerator.AddToIdleTargets(potentialTarget);
            //     }
            // }
        }

        // private void OnTriggerExit2D(Collider2D other)
        // {
        //     Target potentialTarget = other.GetComponent<Target>();
        //     if (potentialTarget != null && potentialTarget.IsTOI)
        //     {
        //         _targetGenerator.RemoveToIdleTargets(potentialTarget);
        //     }
        // }
    }
}
