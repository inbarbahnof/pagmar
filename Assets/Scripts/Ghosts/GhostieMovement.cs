using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ghosts
{
    public class GhostieMovement : MonoBehaviour
    {
        private enum MovementType { Circle, StraightLine }

        [SerializeField] private float radius = 2.5f;
        [SerializeField] private MovementType movementType = MovementType.Circle;

        [SerializeField] protected float speed = 1.5f;

        [Header("Run Away From Dog Properties")]
        [SerializeField] private Vector3 runAwayPoint;
        [SerializeField] private float runAwaySpeed = 5f;

        protected Vector3 _initialPosition;
        private Rigidbody2D _rb;
        
        private Vector3 _target;
        private bool _isRunningAway = false;
        private bool _isMoving = false;

        protected void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _initialPosition = transform.position;
            MoveAround();
        }

        public virtual void MoveAround()
        {
            _isRunningAway = false;
            _isMoving = true;

            SetNextTarget();
        }

        public virtual bool StopGoingAround()
        {
            if (_isRunningAway) return true;

            _isMoving = false;
            return false;
        }

        public void MoveAwayFromDog()
        {
            StopGoingAround();
            _isRunningAway = true;
            _isMoving = true;

            _target = runAwayPoint;
        }

        private void FixedUpdate()
        {
            if (!_isMoving) return;

            float currentSpeed = _isRunningAway ? runAwaySpeed : speed;

            _rb.MovePosition(Vector2.MoveTowards(
                _rb.position,
                _target,
                currentSpeed * Time.fixedDeltaTime
            ));

            if (Vector2.Distance(_rb.position, _target) < 0.05f)
            {
                if (_isRunningAway)
                {
                    _isRunningAway = false;
                    MoveAround();
                }
                else
                {
                    SetNextTarget();
                }
            }
        }

        private void SetNextTarget()
        {
            if (movementType == MovementType.Circle)
            {
                Vector2 offset = Random.insideUnitCircle * radius;
                _target = _initialPosition + new Vector3(offset.x, offset.y, 0);
            }
            else
            {
                Vector2 direction = Random.insideUnitCircle.normalized;
                _target = _initialPosition + new Vector3(direction.x, 0, 0) * radius;
            }
        }
    }
}
