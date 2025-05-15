using UnityEngine;
using DG.Tweening;

namespace Ghosts
{
    public class GhostMovement : GhostieMovement
    {
        // [SerializeField] protected float speed = 1.5f;
        // [SerializeField] protected Ease movementEase = Ease.InOutSine;

        [SerializeField] private Transform pointA;
        [SerializeField] private Transform pointB;

        private Tween moveTween;
        private bool movingToB = true;
        private bool isRunning = true;

        private void Start()
        {
            MoveAround();
        }

        public override void MoveAround()
        {
            print("moving around");
            isRunning = true;
            MoveBetweenPoints();
        }

        private void MoveBetweenPoints()
        {
            if (!isRunning) return;

            Transform target = movingToB ? pointB : pointA;

            float distance = Vector3.Distance(transform.position, target.position);
            float duration = distance / speed;

            moveTween = transform.DOMove(target.position, duration)
                .SetEase(movementEase)
                .OnComplete(() =>
                {
                    moveTween = null;
                    SwitchDirection();
                });
        }

        private void SwitchDirection()
        {
            movingToB = !movingToB;
            MoveBetweenPoints();
        }

        public override bool StopGoingAround()
        {
            isRunning = false;

            if (moveTween != null && moveTween.IsActive())
            {
                print("killing tween");
                moveTween.Kill();
                moveTween = null;
            }

            return false;
        }
    }
}