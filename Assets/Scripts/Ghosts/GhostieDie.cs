using UnityEngine;

namespace Ghosts
{
    public class GhostieDie : MonoBehaviour
    {
        [SerializeField] private GameObject _intireGhostie;
        [SerializeField] private GameObject _detecror;
        [SerializeField] private GhostieMovement _movement;
        
        private AttackDetector _attackDetector;
        private Vector3 _initialPos;

        private void Awake()
        {
            _attackDetector = GetComponent<AttackDetector>();
            _initialPos = _intireGhostie.transform.position;
        }
        
        public void Die()
        {
            _detecror.SetActive(false);
            _movement.Die();
            _attackDetector.IsAttackEnabled(false);
        }

        public void Live()
        {
            _detecror.SetActive(true);
            _movement.ResetMovement();
            _attackDetector.IsAttackEnabled(true);
            
            _intireGhostie.transform.position = _initialPos;
        }
    }
}