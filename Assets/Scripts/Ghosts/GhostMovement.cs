using System.Collections;
using UnityEngine;

namespace Ghosts
{
    public class GhostMovement : GhostieMovement
    {
        [SerializeField] private Transform pointA;
        [SerializeField] private Transform pointB;
        [SerializeField] private float _pauseAfterTarget = 2f;

        private bool movingToB = true;
        private bool isGoingAround = true;
        private bool isGoingToTarget = false;
        
        private Vector2 _velocity = Vector2.zero;
        private float _smoothTime = 0.4f;

        private Vector3 _currentTarget;

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
        
            Vector2 newPos = Vector2.SmoothDamp(
                _rb.position,
                _currentTarget,
                ref _velocity,
                _smoothTime,
                speed,
                Time.fixedDeltaTime
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

        // private void FixedUpdate()
        // {
        //     if (!isGoingAround && !isGoingToTarget) return;
        //
        //     Vector2 newPos = Vector2.MoveTowards(
        //         _rb.position,
        //         _currentTarget,
        //         speed * Time.fixedDeltaTime
        //     );
        //
        //     _rb.MovePosition(newPos);
        //
        //     if (Vector2.Distance(_rb.position, _currentTarget) < 0.05f)
        //     {
        //         if (isGoingToTarget)
        //         {
        //             isGoingToTarget = false;
        //             StartCoroutine(WaitAndResume());
        //         }
        //         else if (isGoingAround)
        //         {
        //             SwitchDirection();
        //         }
        //     }
        // }

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
            isGoingToTarget = false;
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
