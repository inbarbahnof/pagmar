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
            if (other.CompareTag("Player"))
            {
                print("player died with ghost");
                _attack.StopAttacking(false);
                _movement.MoveAround();
            }
            
            base.OnTriggerEnter2D(other);
        }
    }
}
