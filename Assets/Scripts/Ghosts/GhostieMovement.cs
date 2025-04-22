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
        
        private Vector3 _initialPosition;
        private Vector3 _movementLine;
        
        private void Start()
        {
            _initialPosition = transform.position;

            if (movementType == MovementType.Circle)
                MoveInCircle();
            else
                MoveInStraightLine();
        }

        private void MoveInCircle()
        {
            Vector3 randomOffset = Random.insideUnitCircle * radius;
            Vector3 target = _initialPosition + new Vector3(randomOffset.x, randomOffset.y, 0);

            float distance = Vector3.Distance(transform.position, target);
            float moveDuration = distance / speed;
            
            transform.DOMove(target, moveDuration)
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
            
            transform.DOMove(target, moveDuration)
                .SetEase(Ease.InOutSine)
                .OnComplete(MoveInStraightLine);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            
            if (movementType == MovementType.Circle)
                Gizmos.DrawSphere(_initialPosition, radius);
            else
                Gizmos.DrawLine(_initialPosition,_movementLine);
        }
    }
}
