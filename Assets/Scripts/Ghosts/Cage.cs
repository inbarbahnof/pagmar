using System;
using UnityEngine;

namespace Ghosts
{
    public class Cage : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ghostie"))
            {
                other.GetComponent<GhostieDie>().Die();
            }
        }
    }
}
