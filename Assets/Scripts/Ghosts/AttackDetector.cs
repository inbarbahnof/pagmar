using System;
using UnityEngine;

namespace Ghosts
{
    public class AttackDetector : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                print("player got attacked");
            }
        }
    }
}
