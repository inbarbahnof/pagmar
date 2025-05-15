using System;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace Ghosts
{
    public class GhostieMovement : MonoBehaviour
    {
        private enum MovementType { Circle, StraightLine }

        [SerializeField] private float radius = 2.5f;
        [SerializeField] private MovementType movementType = MovementType.Circle;
        
        [SerializeField] protected float speed = 1.5f;
        [SerializeField] protected Ease movementEase = Ease.InOutSine;

        [Header("Run Away From Dog Properties")]
        [SerializeField] private Vector3 runAwayPoint;
        [SerializeField] private float runAwaySpeed = 5f;
        
        protected Vector3 _initialPosition;
        private Tween curTween;
        private Tween runAwayTween;
        private bool _isRunningAway;
        
        private void Start()
        {
            _initialPosition = transform.position;
            MoveAround();
        }

        public virtual void MoveAround()
        {
            if (movementType == MovementType.Circle)
                MoveInCircle();
            else
                MoveInStraightLine();
        }

        public virtual bool StopGoingAround()
        {
            if (_isRunningAway) return true;
            
            if (curTween != null)
            {
                curTween.Kill();
                curTween = null;
            }

            return false;
        }

        public void MoveAwayFromDog()
        {
            StopGoingAround();
            // print("moving away from dog");
            _isRunningAway = true;
            
            float distance = Vector3.Distance(transform.position, runAwayPoint);
            float moveDuration = distance / runAwaySpeed;

            runAwayTween = transform.DOMove(runAwayPoint, moveDuration)
                .SetEase(movementEase)
                .OnComplete(() =>
                {
                    runAwayTween = null;
                    _isRunningAway = false;
                    // print("stop moving away from dog");
                    MoveAround();
                });
        }

        private void MoveInCircle()
        {
            Vector3 randomOffset = Random.insideUnitCircle * radius;
            Vector3 target = _initialPosition + new Vector3(randomOffset.x, randomOffset.y, 0);

            float distance = Vector3.Distance(transform.position, target);
            float moveDuration = distance / speed;
            
            curTween = transform.DOMove(target, moveDuration)
                .SetEase(movementEase)
                .OnComplete(MoveInCircle);
        }

        private void MoveInStraightLine()
        {
            Vector2 direction = Random.insideUnitCircle.normalized;
            Vector3 target = _initialPosition + new Vector3(direction.x, 0, 0) * radius;
            
            float distance = Vector3.Distance(transform.position, target);
            float moveDuration = distance / speed;
            
            curTween = transform.DOMove(target, moveDuration)
                .SetEase(Ease.InOutSine)
                .OnComplete(MoveInStraightLine);
        }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.blue;
        //     
        //     if (movementType == MovementType.Circle)
        //         Gizmos.DrawSphere(_initialPosition, radius);
        //     else
        //         Gizmos.DrawLine(_initialPosition,_movementLine);
        // }
    }
}
