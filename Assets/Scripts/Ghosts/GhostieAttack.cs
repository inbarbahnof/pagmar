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

        public void StopAttacking()
        {
            _attacking = false;
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

                if (Vector2.Distance(transform.position, _targetPlayer.position) <= 0.5f)
                {
                    Debug.Log("Attack reached!");
                    _attacking = false;
                    _ghostieMovement.MoveAround();
                }
                else
                {
                    _rb.MovePosition(newPos);
                }
            }
        }
    }
}
