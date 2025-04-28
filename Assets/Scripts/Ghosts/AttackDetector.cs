using System;
using UnityEngine;

namespace Ghosts
{
    public class AttackDetector : PlayerDeathZone
    {
        [SerializeField] private GhostieAttack _attack;
        [SerializeField] private GhostieMovement _movement;
        
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            
            if (other.CompareTag("Player"))
            {
                _attack.StopAttacking(false);
                _movement.MoveAround();
                print("player died with ghost");
            }
        }
    }
}
