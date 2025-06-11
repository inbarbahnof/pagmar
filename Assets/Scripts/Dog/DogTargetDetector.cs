using System;
using UnityEngine;
using Targets;

namespace Dog
{
    public class DogTargetDetector : MonoBehaviour
    {
        [SerializeField] private DogActionManager _actionManager;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Food"))
            {
                // print("food is close to detector");
                _actionManager.FoodIsClose(other);
            }
            else if (other.CompareTag("GhostieDetector"))
            {
                TargetGenerator.instance.AddToGhostiesTargets(other.GetComponent<Target>());
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Food"))
            {
                _actionManager.FoodIsFar(other);
            }
            else if (other.CompareTag("GhostieDetector"))
            {
                TargetGenerator.instance.RemoveFromGhostiesTargets(other.GetComponent<Target>());
            }
        }
    }
}
