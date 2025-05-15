using UnityEngine;
using DG.Tweening;

namespace Ghosts
{
    public class GhostMovement : GhostieMovement
    {
        [SerializeField] private Transform pointA;
        [SerializeField] private Transform pointB;

        private Tween moveTween;
        private bool movingToB = true;

        private void Start()
        {
            MoveAround();
        }

        public override void MoveAround()
        {
            print("moving around");
            MoveBetweenPoints();
        }

        private void MoveBetweenPoints()
        {
            Transform target = movingToB ? pointB : pointA;

            float distance = Vector3.Distance(transform.position, target.position);
            float duration = distance / speed;

            moveTween = transform.DOMove(target.position, duration)
                .SetEase(movementEase)
                .OnComplete(SwitchDirection);
        }

        private void SwitchDirection()
        {
            movingToB = !movingToB;
            MoveBetweenPoints();
        }

        public override bool StopGoingAround()
        {
            print("stop going around (moveTween != null) " + (moveTween != null) + " moveTween.IsActive() " + moveTween.IsActive());
            if (moveTween != null)
            {
                print("killing tween");
                moveTween.Kill();
                moveTween = null;
            }

            return false;
        }
    }
}