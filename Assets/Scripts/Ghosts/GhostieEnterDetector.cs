using System;
using UnityEngine;

namespace Ghosts
{
    public class GhostieEnterDetector : MonoBehaviour
    {
        [SerializeField] private GhostieAttack _attack;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && 
                !other.GetComponent<PlayerStealthManager>().isProtected)
            {
                _attack.Attack(other.transform);
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if(other.CompareTag("Dog"))
            {
                _attack.StopAttacking(true, other.transform.position);
            }
        }
    }
}
