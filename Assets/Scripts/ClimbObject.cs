using System;
using System.Collections;
using UnityEngine;

public class ClimbObject : MonoBehaviour
{
    [SerializeField] private Transform _whereToClimbPhase1;
    [SerializeField] private Transform _whereToClimbPhase2;

    private float _waitForClimb;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStateManager manager = other.GetComponent<PlayerStateManager>();
            manager.UpdateClimbing(_whereToClimbPhase1, _whereToClimbPhase2);
        }
    }
}
