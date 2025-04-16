using System;
using UnityEngine;

namespace Targets
{
    public class TargetRangeManager : MonoBehaviour
    {
        [SerializeField] private RandomTargetGenerator randomTargetGenerator;

        private void Awake()
        {
            randomTargetGenerator.AddTargets(GetComponentInParent<Target>());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Target target = other.GetComponent<Target>();
            if (target != null)
            {
                // Debug.Log("added target: " + other.name);
                randomTargetGenerator.AddTargets(target);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Target target = other.GetComponent<Target>();
            if (target != null)
            {
                // Debug.Log("removed target: " + other.name);
                randomTargetGenerator.RemoveTarget(target);
            }
        }
    }
}
