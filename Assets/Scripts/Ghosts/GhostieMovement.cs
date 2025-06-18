using System;
using System;
using System.Collections;
using DG.Tweening;
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
        [SerializeField] private bool _goesToRunPos;
        [SerializeField] private float _distanceToRun = 2;
        [SerializeField] private float _waitForIdle = 1.5f;
        [SerializeField] private Transform runAwayPoint;
        [SerializeField] private float runAwaySpeed = 5f;

        protected Vector3 _initialPosition;
        protected Rigidbody2D _rb;
        
        private Vector3 _target;
        private bool _isRunningAway = false;
        private bool _isMoving = false;
        private bool _dead;

        private Coroutine _coroutine;

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

        public void Die(Vector3 pos, Cage cage)
        {
            _isMoving = false;
            if (_coroutine != null) StopCoroutine(_coroutine);
            _dead = true;
            
            transform.DOMove(pos, 1f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    cage.InvokeMethod();
                    transform.SetParent(cage.transform);
                });
        }

        public void ResetMovement()
        {
            // print("ResetMovement");
            _isMoving = true;
            _dead = false;
            MoveAround();
        }
        
        public void MoveAwayFromDog(Vector3 dogPosition)
        {
            StopGoingAround();
            _isRunningAway = true;
            _isMoving = true;

            if (_goesToRunPos)
            {
                _target = runAwayPoint.position;
            }
            else
            {
                Vector3 directionAwayFromDog = (transform.position - dogPosition).normalized;
                Vector3 proposedTarget = transform.position + directionAwayFromDog * _distanceToRun;
                
                _target = EscapePositionManager.Instance.GetClosestEscapePoint(proposedTarget, transform.position);
            }
        }

        public void MoveToPos(Vector3 pos)
        {
            _isRunningAway = true;
            _isMoving = true;
            _dead = false;
            _target = pos;
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
                if (_isRunningAway && !_dead)
                {
                    _isMoving = false;
                    if (_coroutine != null) StopCoroutine(_coroutine);
                    _coroutine = StartCoroutine(WaitToGoBackToIdle());
                }
                else
                {
                    SetNextTarget();    
                }
            }
        }

        private IEnumerator WaitToGoBackToIdle()
        {
            yield return new WaitForSeconds(_waitForIdle);
            
            _isMoving = true;
            _isRunningAway = false;
            _dead = false;
            MoveAround();
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
