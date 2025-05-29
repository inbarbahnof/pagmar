using System;
using Interactables;
using UnityEngine;

public class PlayerReached : MonoBehaviour
{
    [SerializeField] private DogWaitForPlayer _dogWait;
    [SerializeField] private Obstacle _obstacle;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _dogWait.PlayerReached();
            if (_obstacle != null) _obstacle.PlayerReachedTarget();
        }
    }
}
