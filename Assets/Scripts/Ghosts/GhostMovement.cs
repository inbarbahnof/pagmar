using UnityEngine;

namespace Ghosts
{
    public class GhostMovement : GhostieMovement
    {
        [SerializeField] private Transform pointA;
        [SerializeField] private Transform pointB;

        private bool movingToB = true;
        private bool isGoingAround = true;

        private Rigidbody2D _rb;
        private Vector3 _currentTarget;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            
            MoveAround();
        }

        public override void MoveAround()
        {
            isGoingAround = true;
            SetNextTarget();
        }

        private void FixedUpdate()
        {
            if (!isGoingAround) return;

            Vector2 newPos = Vector2.MoveTowards(
                _rb.position,
                _currentTarget,
                speed * Time.fixedDeltaTime
            );

            _rb.MovePosition(newPos);

            // Check if reached target
            if (Vector2.Distance(_rb.position, _currentTarget) < 0.05f)
            {
                SwitchDirection();
            }
        }

        private void SetNextTarget()
        {
            _currentTarget = (movingToB ? pointB : pointA).position;
        }

        private void SwitchDirection()
        {
            movingToB = !movingToB;
            SetNextTarget();
        }

        public override bool StopGoingAround()
        {
            isGoingAround = false;
            return false;
        }
    }
}