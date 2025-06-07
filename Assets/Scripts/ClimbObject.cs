using System;
using System.Collections;
using UnityEngine;

public class ClimbObject : MonoBehaviour
{
    private float _waitForClimb;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStateManager manager = other.GetComponent<PlayerStateManager>();
            manager.UpdateClimbing();
        }
    }
}
