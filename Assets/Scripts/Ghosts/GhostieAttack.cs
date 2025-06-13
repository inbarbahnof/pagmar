using System;
using UnityEngine;
using DG.Tweening;

namespace Ghosts
{
    public class GhostieAttack : MonoBehaviour
    {
        [SerializeField] protected float attackSpeed = 4.5f; 
        [SerializeField] protected float _attackRadius = 7f;
        [SerializeField] protected bool _keepDistance;
        protected float _desiredDistanceFromPlayer = 5f;

        protected Rigidbody2D _rb;
        protected GhostieMovement _ghostieMovement;
        protected Transform _targetPlayer;
        protected bool _attacking = false;
        protected Vector3 _initialPos;
        protected Vector2 _attackOffset;
        
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

        public void SetKeepDistance(bool keep)
        {
            _keepDistance = keep;
        }
        
        public void SetAttackSpeed(float speed)
        {
            attackSpeed = speed;
        }
        
        public void MoveToPos(Vector3 pos)
        {
            _attacking = false;
            _targetPlayer = null;
            
            _ghostieMovement.MoveToPos(pos);
        }
        
        public virtual void Attack(Transform player)
        {
            // print("in attack");
            
            if (player == null || _attacking) return;
            
            bool isRunning = _ghostieMovement.StopGoingAround();
            
            _attackOffset = UnityEngine.Random.insideUnitCircle.normalized;

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
                float currentDistance = Vector3.Distance(transform.position, _targetPlayer.position);
                float initialDistance = Vector3.Distance(transform.position, _initialPos);

                if (initialDistance <= _attackRadius && currentDistance <= _attackRadius)
                {
                    if (_keepDistance) // When keeping distance, stop moving if close enough
                    {
                        if (currentDistance > _desiredDistanceFromPlayer)
                        {
                            Vector3 targetPosWithOffset = _targetPlayer.position + (Vector3)_attackOffset;
                            
                            Vector2 newPos = Vector2.MoveTowards(
                                transform.position,
                                targetPosWithOffset,
                                attackSpeed * Time.fixedDeltaTime
                            );
                            _rb.MovePosition(newPos);
                        }
                        else
                        {
                            _rb.linearVelocity = Vector2.zero;
                        }
                    }
                    else
                    {
                        Vector2 newPos = Vector2.MoveTowards(
                            transform.position,
                            _targetPlayer.position,
                            attackSpeed * Time.fixedDeltaTime
                        );
                        _rb.MovePosition(newPos);
                    }
                }
                else
                {
                    StopAttacking(false, Vector3.zero);
                }
            }
        }
    }
}
