using System;
using UnityEngine;

namespace Interactables
{
    public class CircleToMoveGhosties : MonoBehaviour
    {
        [SerializeField] private GhostiesCircleObstacle manager;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Dog"))
            {
                manager.GhostiesRun();
            }
        }
    }
}