using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomTargetGenerator : MonoBehaviour
{
    private List<Target> targets = new List<Target>(); // player target added on start
    
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
    
    public Target GenerateNewTarget()
    {
        Target newTarget = targets[GenerateNewTargetIndex()];
        return newTarget;
    }

    private int GenerateNewTargetIndex()
    {
        return Random.Range(0, targets.Count);
    }
    
}
