using System.Collections;
using UnityEngine;

namespace Ghosts
{
    public class GhostMovement : GhostieMovement
    {
        [SerializeField] private Transform pointA;
        [SerializeField] private Transform pointB;
        [SerializeField] private float _pauseAfterTarget = 1f;

        private bool movingToB = true;
        private bool isGoingAround = true;
        private bool isGoingToTarget = false;

        //private Rigidbody2D _rb;
        private Vector3 _currentTarget;

        // private void Awake()
        // {
        //     _rb = GetComponent<Rigidbody2D>();
        // }
        // this already happens in parent class

        private void Start()
        {
            MoveAround();
        }

        public override void MoveAround()
        {
            isGoingAround = true;
            isGoingToTarget = false;
            SetNextTarget();
        }

        private void FixedUpdate()
        {
            if (!isGoingAround && !isGoingToTarget) return;

            Vector2 newPos = Vector2.MoveTowards(
                _rb.position,
                _currentTarget,
                speed * Time.fixedDeltaTime
            );

            _rb.MovePosition(newPos);

            if (Vector2.Distance(_rb.position, _currentTarget) < 0.05f)
            {
                if (isGoingToTarget)
                {
                    isGoingToTarget = false;
                    StartCoroutine(WaitAndResume());
                }
                else if (isGoingAround)
                {
                    SwitchDirection();
                }
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

        public void GoToTargetAndPause(Transform target)
        {
            StopGoingAround();
            isGoingToTarget = true;
            _currentTarget = target.position;
        }

        private IEnumerator WaitAndResume()
        {
            yield return new WaitForSeconds(_pauseAfterTarget);
            MoveAround();
        }
    }
}
