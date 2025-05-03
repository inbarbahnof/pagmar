using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Targets
{
    public class TargetRangeManager : MonoBehaviour
    {
        [FormerlySerializedAs("randomTargetGenerator")] [SerializeField] private TargetGenerator targetGenerator;

        private void Awake()
        {
            targetGenerator.AddToPlayerTargets(GetComponentInParent<Target>());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Target target = other.GetComponent<Target>();
            if (target != null)
            {
                // Debug.Log("added target: " + other.name);
                targetGenerator.AddToPlayerTargets(target);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Target target = other.GetComponent<Target>();
            if (target != null)
            {
                // Debug.Log("removed target: " + other.name);
                targetGenerator.RemoveFromPlayerTarget(target);
            }
        }
    }
}
