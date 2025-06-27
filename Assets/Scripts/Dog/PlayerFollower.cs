using System;
using System.Collections;
using System.Collections.Generic;
using Targets;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Dog
{
    public class PlayerFollower : MonoBehaviour
    {
        public event Action OnIdleAfterTarget;
        public event Action OnStartFollowTOI;
        public event Action OnPerformingTargetAction;
        public event Action OnFinishedTargetAction;
        public event Action OnStartFollow;

        [SerializeField] private Transform player;
        [SerializeField] private DogAnimationManager _animationManager;
        [SerializeField] private float _walkSpeed = 3.5f;
        [SerializeField] private float _runSpeed = 7.5f;
        [SerializeField] private float _crouchSpeed = 2.8f;
        [SerializeField] private NavMeshAgent agent;
        
        private Target currentTarget;
        private Target nextTarget;
        private float targetDistance;
        private bool isPerformingAction = false;
        private bool isGoingToTarget = false;

        private bool _isRunning;

        private float stopProb = 0.7f;
        private float initialStopProb;

        public bool IsRunning => _isRunning;

        private DogActionManager _actionManager;

        private void Start()
        {
            _actionManager = GetComponent<DogActionManager>();
            
            agent.updateRotation = false;
            agent.updateUpAxis = false;

            initialStopProb = stopProb;
            agent.speed = _walkSpeed;
        }

        void Update()
        {
            if (currentTarget == null || isPerformingAction)
                return;

            float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
            // print("agent.isStopped " + agent.isStopped);
            // Check if the dog reached the target
            // print("distance to target " + distance + currentTarget.name);
            if (distance <= targetDistance && isGoingToTarget)   
            {
                agent.isStopped = true;
                StartTargetAction();
            }
            else if (isGoingToTarget)
            {
                // print("going to target " + currentTarget.name);
                agent.isStopped = false;
                agent.SetDestination(currentTarget.transform.position);
            }
        }

        public void SetStealth(bool stealth)
        {
            if (stealth) agent.speed = _crouchSpeed;
            else agent.speed = _walkSpeed;
        }

        public void SetSpeed(bool walking)
        {
            if (walking)
            {
                _isRunning = false;
                agent.speed = _walkSpeed;
            }
            else
            {
                _isRunning = true;
                agent.speed = _runSpeed;
            }
        }

        public void GoToNextTarget()
        {
            currentTarget = nextTarget;
            nextTarget = null;

            StartCoroutine(WaitOnGoingToPlayer());

            OnStartFollow?.Invoke();

            isGoingToTarget = true;
            isPerformingAction = false;
            targetDistance = currentTarget.GetDistance();
            currentTarget.OnTargetActionComplete += OnTargetActionComplete;
        }

        public void SetIsGoingToTarget(bool going)
        {
            isGoingToTarget = going;
        }

        public void ResetToCheckpoint(Vector2 position)
        {
            transform.position = position;
            
            currentTarget = null;
            nextTarget = null;
            
            stopProb = initialStopProb;
            
            isGoingToTarget = true;
            isPerformingAction = false;
        }

        private float ComputeStopProbOnCall()
        {
            switch (GameManager.instance.ConnectionState)
            {
                case 1:
                    return 0.4f;
                case 2:
                    return 0.3f;
                case 3:
                    return 0.2f;
                case 4:
                    return 0.1f;
            }
            return 0.5f;
        }

        private void GoToTarget(Target target)
        {
            if (currentTarget != null)
                currentTarget.OnTargetActionComplete -= OnTargetActionComplete;

            currentTarget = target;
            nextTarget = null;

            isGoingToTarget = true;
            isPerformingAction = false;
            
            OnStartFollow?.Invoke();

            targetDistance = currentTarget.GetDistance();
            currentTarget.OnTargetActionComplete += OnTargetActionComplete;
        }
        
        public void PauseAgentMovement()
        {
            if (agent != null)
            {
                agent.isStopped = true;
            }

            isGoingToTarget = false;
        }
        
        public void ResumeAgentMovement()
        {
            if (currentTarget == null || isPerformingAction) return;

            isGoingToTarget = true;
            agent.isStopped = false;
            agent.SetDestination(currentTarget.transform.position);
        }

        public void GoToCallTarget(Target target)
        {
            stopProb = ComputeStopProbOnCall();
            GoToTarget(target);
        }
        
        public void GoToFoodTarget(Target target)
        {
            stopProb = 0;
            GoToTarget(target);
        }

        public void StopGoingToTarget()
        {
            if (currentTarget != null)
                currentTarget.OnTargetActionComplete -= OnTargetActionComplete;

            currentTarget = null;
            agent.isStopped = true;
        }

        public void SetStopProb(bool isStoping)
        {
            if (isStoping)
            {
                stopProb = 0;
            }
            else
            {
                stopProb = initialStopProb;
            }
        }

        public void SetNextTarget(Target target)
        {
            nextTarget = target;
        }

        private IEnumerator WaitOnGoingToPlayer()
        {
            yield return new WaitForSeconds(1.5f);
        }

        public void GoToTOI(Target potentialTarget)
        {
            if (currentTarget != null)
                currentTarget.OnTargetActionComplete -= OnTargetActionComplete;

            nextTarget = currentTarget;
            currentTarget = potentialTarget;
            targetDistance = currentTarget.GetDistance();
            currentTarget.OnTargetActionComplete += OnTargetActionComplete;
            isPerformingAction = false;
            isGoingToTarget = true;

            OnStartFollowTOI?.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Target potentialTarget = other.GetComponent<Target>();
            if (potentialTarget != null && potentialTarget.IsTOI)
            {
                if (_actionManager.CurState != DogState.ChaseGhostie && Random.value < stopProb)
                {
                    GoToTOI(potentialTarget);
                }
            }
        }

        private void StartTargetAction()
        {
            //print("in StartTargetAction");
            isPerformingAction = true;
            isGoingToTarget = false;
            
            OnPerformingTargetAction?.Invoke();
            currentTarget.StartTargetAction(this);
        }

        private void OnTargetActionComplete()
        {
            OnFinishedTargetAction?.Invoke();
            
            if(stopProb != initialStopProb) stopProb = initialStopProb;
            
            isPerformingAction = false;
            if (currentTarget != null) currentTarget.OnTargetActionComplete -= OnTargetActionComplete;

            if (nextTarget != null)
            {
                GoToNextTarget();
            }
            else
            {
                // notify that target action is complete
                OnIdleAfterTarget?.Invoke();
            }
        }
    }
}
