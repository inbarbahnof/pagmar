using System;
using Dog;
using UnityEngine;

namespace Ghosts
{
    public class DeathAttackDetector : MonoBehaviour
    {
        [SerializeField] private GhostieAttack _attack;
        [SerializeField] private GhostieMovement _movement;
        [SerializeField] private bool _canAttackDog;

        private bool _canAttack = true;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_canAttack) return;
            
            if (other.CompareTag("Player"))
            {
                _attack.StopAttacking(false, Vector3.zero);
                _movement.MoveAround();
                
                if (!other.GetComponent<PlayerStealthManager>().isProtected)
                {
                    print("player died with ghost");
                    GameManager.checkpointManagerInstance.Undo();
                }
            }
            else if (_canAttackDog && other.CompareTag("Dog") && !other.GetComponent<DogActionManager>().IsDogProtected)
            {
                _attack.StopAttacking(false, Vector3.zero);
                _movement.MoveAround();
                GameManager.checkpointManagerInstance.Undo();
            }
        }

        public void IsAttackEnabled(bool enabled)
        {
            // print("IsAttackEnabled " + name + " " + enabled);
            _canAttack = enabled;
        }

        public void SetCanAttackDog(bool can)
        {
            _canAttackDog = can;
        }
    }
}
