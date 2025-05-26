using System;
using UnityEngine;

namespace Ghosts
{
    public class AttackDetector : PlayerDeathZone
    {
        [SerializeField] private GhostieAttack _attack;
        [SerializeField] private GhostieMovement _movement;
        [SerializeField] private bool _canAttackDog;
        
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                print("player died with ghost");
                _attack.StopAttacking(false);
                _movement.MoveAround();
            }
            else if (_canAttackDog && other.CompareTag("Dog"))
            {
                _attack.StopAttacking(false);
                _movement.MoveAround();
                DogDied();
            }
            
            base.OnTriggerEnter2D(other);
        }
    }
}
