using System;
using UnityEngine;

namespace Interactables
{
    public class PlayerReachedBush : MonoBehaviour
    {
        [SerializeField] private Stealth3Obstacle _manager;
        [SerializeField] private bool isNext;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (!isNext) _manager.PlayerReachedStealth();
                else _manager.PlayerReachedNextTarget();
            }
        }
    }
}