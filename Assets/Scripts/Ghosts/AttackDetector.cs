using System;
using Dog;
using UnityEngine;

namespace Ghosts
{
    public class AttackDetector : PlayerDeathZone
    {
        [SerializeField] private GhostieAttack _attack;
        [SerializeField] private GhostieMovement _movement;
        [SerializeField] private bool _canAttackDog;

        private bool _canAttack = true;
        
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (!_canAttack) return;
            
            if (other.CompareTag("Player") && !other.GetComponent<PlayerStealthManager>().isProtected)
            {
                print("player died with ghost");
                _attack.StopAttacking(false, Vector3.zero);
                _movement.MoveAround();
            }
            else if (_canAttackDog && other.CompareTag("Dog") && !other.GetComponent<DogActionManager>().IsDogProtected)
            {
                _attack.StopAttacking(false, Vector3.zero);
                _movement.MoveAround();
                DogDied();
            }
            
            base.OnTriggerEnter2D(other);
        }

        public void IsAttackEnabled(bool enabled)
        {
            // print("IsAttackEnabled " + name + " " + enabled);
            _canAttack = enabled;
        }
    }
}
