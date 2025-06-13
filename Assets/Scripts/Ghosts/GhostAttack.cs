using System;
using UnityEngine;
using DG.Tweening;

namespace Ghosts
{
    public class GhostAttack : GhostieAttack
    {
        [SerializeField] private GhostMovement _ghostMovement;
        
        public override void StopAttacking(bool isRunning, Vector3 dogPos)
        {
            _attacking = false;
            _targetPlayer = null;
            
            _ghostMovement.MoveAround();
        }
        
        public override void Attack(Transform player)
        {
            // print("in attack " + player.name + " attacking " + _attacking);
            
            if (player == null || _attacking) return;

            _ghostMovement.StopGoingAround();
            
            _targetPlayer = player;
            _attacking = true;
        }
    }
}
