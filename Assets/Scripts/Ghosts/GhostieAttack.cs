using System;
using UnityEngine;
using DG.Tweening;

namespace Ghosts
{
    public class GhostieAttack : MonoBehaviour
    {
        [SerializeField] protected float attackSpeed = 4.5f; 
        [SerializeField] protected float _attackRadius = 7f;

        protected Rigidbody2D _rb;
        protected GhostieMovement _ghostieMovement;
        protected Transform _targetPlayer;
        protected bool _attacking = false;
        protected Vector3 _initialPos;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _ghostieMovement = GetComponent<GhostieMovement>();
            _initialPos = transform.position;
        }

        public virtual void StopAttacking(bool isRunning, Vector3 dogPosition)
        {
            _attacking = false;
            _targetPlayer = null;
            
            if (isRunning)
            {
                _ghostieMovement.MoveAwayFromDog(dogPosition);
            }
            else
            {
                _ghostieMovement.MoveAround();
            }
        }
        
        public virtual void Attack(Transform player)
        {
            // print("in attack");
            
            if (player == null || _attacking) return;
            
            bool isRunning = _ghostieMovement.StopGoingAround();

            if (!isRunning)
            {
                _targetPlayer = player;
                _attacking = true;
            }
        }

        protected virtual void FixedUpdate()
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
                    StopAttacking(false, Vector3.zero);
                }
            }
        }
    }
}
