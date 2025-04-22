using System;
using UnityEngine;

namespace Ghosts
{
    public class GhostEnterDetector : MonoBehaviour
    {
        [SerializeField] private GhostieAttack _attack;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) 
            {
                _attack.Attack(other.transform);
            }
        }
    }
}
