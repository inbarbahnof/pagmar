using System;
using System.Collections;
using System.Collections.Generic;
using Targets;
using UnityEngine;
using UnityEngine.AI;
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
        
        private NavMeshAgent agent;
        private Target currentTarget;
        private Target nextTarget;
        private float targetDistance;
        private bool isPerformingAction = false;
        private bool isGoingToTarget = false;
        // private bool isTargetActionDone = false;

        private float stopProb = 0.7f;
        private float initialStopProb;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;

            initialStopProb = stopProb;
        }

        void Update()
        {
            if (currentTarget == null || isPerformingAction)
                return;

            float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
            
            // print("agent remaininig distance " + agent.remainingDistance);
            // if (agent.hasPath && agent.remainingDistance <= agent.stoppingDistance + 0.1f && isGoingToTarget)
            // {
            //     print("agent reached target");
            // }
            
            // Check if the dog reached the target
            if (distance <= targetDistance && isGoingToTarget)   
            {
                agent.isStopped = true;
                StartTargetAction();
            }
            else if (isGoingToTarget)
            {
                agent.isStopped = false;
                agent.SetDestination(currentTarget.transform.position);
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

        public void GoToCallTarget(Target target)
        {
            stopProb = 0;
            
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            Target potentialTarget = other.GetComponent<Target>();
            if (potentialTarget != null && potentialTarget.IsTOI)
            {
                if (Random.value < stopProb)
                {
                    // Debug.Log("Switching to new target from trigger " + potentialTarget.name);

                    // Unsubscribe from old target
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
            }
        }

        private void StartTargetAction()
        {
            //print("in StartTargetAction");
            isPerformingAction = true;
            isGoingToTarget = false;
            
            OnPerformingTargetAction?.Invoke();
            currentTarget.StartTargetAction();
        }

        private void OnTargetActionComplete()
        {
            OnFinishedTargetAction?.Invoke();
            
            if(stopProb != initialStopProb) stopProb = initialStopProb;
            
            isPerformingAction = false;
            currentTarget.OnTargetActionComplete -= OnTargetActionComplete;

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
