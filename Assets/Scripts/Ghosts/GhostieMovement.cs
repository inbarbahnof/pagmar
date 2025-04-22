using System;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace Ghosts
{
    public class GhostieMovement : MonoBehaviour
    {
        private enum MovementType { Circle, StraightLine }

        [SerializeField] private MovementType movementType = MovementType.Circle;
        [SerializeField] private float radius = 2.5f;
        [SerializeField] private float speed = 1.5f;
        [SerializeField] private float runAwaySpeed = 5f;
        [SerializeField] private float moveAwayFromDogDistance = 4f;
        
        private Vector3 _initialPosition;
        private Vector3 _movementLine;
        private Tween curTween;
        private Tween runAwayTween;
        private bool _isRunningAway;
        
        private void Start()
        {
            _initialPosition = transform.position;

            MoveAround();
        }

        public void MoveAround()
        {
            if (movementType == MovementType.Circle)
                MoveInCircle();
            else
                MoveInStraightLine();
        }

        public bool StopGoingAround()
        {
            if (_isRunningAway) return true;
            
            if (curTween != null)
            {
                curTween.Kill();
                curTween = null;
            }

            return false;
        }

        public void MoveAwayFromDog(Transform dog)
        {
            StopGoingAround();
            print("moving away from dog");
            _isRunningAway = true;
            
            Vector3 awayDir = (transform.position - dog.position).normalized;
            Vector3 target = transform.position + awayDir * moveAwayFromDogDistance;

            float distance = Vector3.Distance(transform.position, target);
            float moveDuration = distance / runAwaySpeed;

            runAwayTween = transform.DOMove(target, moveDuration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    runAwayTween = null;
                    _isRunningAway = false;
                    print("stop moving away from dog");
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
                .SetEase(Ease.InOutSine)
                .OnComplete(MoveInCircle);
        }

        private void MoveInStraightLine()
        {
            Vector2 direction = Random.insideUnitCircle.normalized;
            Vector3 target = _initialPosition + new Vector3(direction.x, 0, 0) * radius;
            _movementLine = target;
            
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
