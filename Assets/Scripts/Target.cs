using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float distance = 0.1f;

    public float GetDistance()
    {
        return distance;
    }

    
}
