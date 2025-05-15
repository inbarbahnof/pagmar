using System;
using UnityEngine;
using DG.Tweening;

namespace Ghosts
{
    public class GhostAttack : GhostieAttack
    {
        [SerializeField] private float attackSpeed = 3f;
         private float _attackRadius = 7f;

        private GhostMovement _ghostMovement;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _ghostMovement = GetComponent<GhostMovement>();
            _initialPos = transform.position;
        }

        public override void StopAttacking(bool isRunning)
        {
            if (isRunning) return;
            
            _attacking = false;
            _targetPlayer = null;
            
            _ghostMovement.MoveAround();
        }
        
        public override void Attack(Transform player)
        {
            print("in attack");
            
            if (player == null || _attacking) return;

            _ghostMovement.StopGoingAround();
            
            _targetPlayer = player;
            _attacking = true;
        }
    }
}
