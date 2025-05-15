using UnityEngine;
using DG.Tweening;

namespace Ghosts
{
    public class GhostMovement : GhostieMovement
    {
        [SerializeField] private Transform pointA;
        [SerializeField] private Transform pointB;

        private Tween moveTween;

        private void Start()
        {
            MoveBetweenPoints();
        }

        public override void MoveAround()
        {
            MoveBetweenPoints();
        }

        private void MoveBetweenPoints()
        {
            float distance = Vector3.Distance(pointA.position, pointB.position);
            float duration = distance / speed;

            moveTween = transform.DOMove(pointB.position, duration)
                .SetEase(movementEase)
                .OnComplete(SwitchDirection);
        }

        private void SwitchDirection()
        {
            // Swap pointA and pointB, then move again
            (pointA, pointB) = (pointB, pointA);
            MoveBetweenPoints();
        }

        public override bool StopGoingAround()
        {
            if (moveTween != null && moveTween.IsActive())
            {
                moveTween.Kill();
                moveTween = null;
            }

            return false;
        }
    }
}