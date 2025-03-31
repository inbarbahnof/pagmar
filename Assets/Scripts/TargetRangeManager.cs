using System;
using UnityEngine;

public class TargetRangeManager : MonoBehaviour
{
    [SerializeField] private RandomTargetGenerator randomTargetGenerator;

    private void Start()
    {
        randomTargetGenerator.AddTargets(GetComponentInParent<Target>());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        randomTargetGenerator.AddTargets(other.GetComponent<Target>());
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        randomTargetGenerator.RemoveTarget(other.GetComponent<Target>());
    }
}
