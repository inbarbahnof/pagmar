using System;
using UnityEngine;
using DG.Tweening;

namespace Ghosts
{
    public class GhostieAttack : MonoBehaviour
    {
        [SerializeField] private float attackSpeed = 3f;

        private Rigidbody2D _rb;
        private GhostieMovement _ghostieMovement;
        private Transform _targetPlayer;
        private bool _attacking = false;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _ghostieMovement = GetComponent<GhostieMovement>();
        }

        public void StopAttacking(bool isRunning)
        {
            _attacking = false;
            
            if (isRunning)
            {
                _ghostieMovement.MoveAwayFromDog();
            }
            else
            {
                _ghostieMovement.MoveAround();
            }
        }
        
        public void Attack(Transform player)
        {
            print("in attack");
            
            if (player == null || _attacking) return;
            
            bool isRunning = _ghostieMovement.StopGoingAround();

            if (!isRunning)
            {
                _targetPlayer = player;
                _attacking = true;
            }
        }

        private void FixedUpdate()
        {
            if (_attacking && _targetPlayer != null)
            {
                Vector2 newPos = Vector2.MoveTowards(
                    transform.position,
                    _targetPlayer.position,
                    attackSpeed * Time.fixedDeltaTime
                );
                
                _rb.MovePosition(newPos);
            }
        }
    }
}
