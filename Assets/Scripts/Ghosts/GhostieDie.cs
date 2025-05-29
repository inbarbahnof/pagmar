using UnityEngine;

namespace Ghosts
{
    public class GhostieDie : MonoBehaviour
    {
        [SerializeField] private GameObject _detecror;
        [SerializeField] private GhostieMovement _movement;
        
        private AttackDetector _attackDetector;


        private void Awake()
        {
            _attackDetector = GetComponent<AttackDetector>();
        }
        
        public void Die()
        {
            _detecror.SetActive(false);
            _movement.Die();
            _attackDetector.DisableAttack();
        }
    }
}