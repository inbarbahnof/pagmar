using System;
using UnityEngine;

namespace Interactables
{ 
    public class ObsFinished : MonoBehaviour
    {
        [SerializeField] private Obstacle _obstacle;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (_obstacle != null) _obstacle.PlayerReachedTarget();
            }
        }
    }
}
