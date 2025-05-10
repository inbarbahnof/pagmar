using System;
using UnityEngine;
using DG.Tweening;

namespace Ghosts
{
    public class GhostieAttack : MonoBehaviour
    {
        [SerializeField] private float attackSpeed = 3f;
         private float _attackRadius = 7f;

        private Rigidbody2D _rb;
        private GhostieMovement _ghostieMovement;
        private Transform _targetPlayer;
        private bool _attacking = false;
        private Vector3 _initialPos;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _ghostieMovement = GetComponent<GhostieMovement>();
            _initialPos = transform.position;
        }

        public void StopAttacking(bool isRunning)
        {
            _attacking = false;
            _targetPlayer = null;
            
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
                if (Vector3.Distance(transform.position, _initialPos) <= _attackRadius
                    && Vector3.Distance(_targetPlayer.position, transform.position) <= _attackRadius )
                {
                    Vector2 newPos = Vector2.MoveTowards(
                        transform.position,
                        _targetPlayer.position,
                        attackSpeed * Time.fixedDeltaTime
                    );
                
                    _rb.MovePosition(newPos);
                }
                else
                {
                    StopAttacking(false);
                }
            }
        }
    }
}
