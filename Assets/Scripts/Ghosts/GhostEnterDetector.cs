using System;
using UnityEngine;

namespace Ghosts
{
    public class GhostEnterDetector : MonoBehaviour
    {
        [SerializeField] private GhostieAttack _attack;
        [SerializeField] private GhostieMovement _movement;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) 
            {
                _attack.Attack(other.transform);
            }
            else if(other.CompareTag("Dog"))
            {
                _movement.MoveAwayFromDog(other.transform);
                _attack.StopAttacking();
            }
        }
    }
}
