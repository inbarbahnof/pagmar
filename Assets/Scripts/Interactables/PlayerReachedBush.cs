using System;
using UnityEngine;

namespace Interactables
{
    public class PlayerReachedBush : MonoBehaviour
    {
        [SerializeField] private Stealth3Obstacle _manager;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _manager.PlayerReachedStealth();
            }
        }
    }
}