using UnityEngine;
using DG.Tweening;

namespace Ghosts
{
    public class GhostieAttack : MonoBehaviour
    {
        [SerializeField] private float attackSpeed = 5f;
        [SerializeField] private Ease attackEase = Ease.InOutSine;

        private bool attacking;
        private Vector3 startAttackPos;
        
        public void Attack(Transform player)
        {
            if (player == null || attacking) return;

            startAttackPos = transform.position;

            float distance = Vector3.Distance(transform.position, player.position);
            float duration = distance / attackSpeed;
            
            attacking = true;

            transform.DOMove(player.position, duration)
                .SetEase(attackEase)
                .OnComplete(() =>
                {
                    GoToStartPos();
                    print("attack complete");
                });
        }

        private void GoToStartPos()
        {
            float distance = Vector3.Distance(startAttackPos, transform.position);
            float duration = distance / attackSpeed;

            transform.DOMove(startAttackPos, duration)
                .SetEase(attackEase)
                .OnComplete(() =>
                {
                    attacking = false; 
                    print("back to place");
                });
        }
    }
}
