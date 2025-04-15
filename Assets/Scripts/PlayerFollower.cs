using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PlayerFollower : MonoBehaviour
{
    public event Action OnIdleAfterTarget;
    public event Action OnStartFollowTOI;
    public event Action OnStartFollow;
    
    [SerializeField] private Transform player;
    
    private NavMeshAgent agent;
    private Target currentTarget;
    private Target nextTarget;
    private float targetDistance;
    private bool isPerformingAction = false;
    private bool isGoingToTarget = true;
    private bool isTargetActionDone = false;

    private float stopProb = 0.5f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
        // GetNewTarget();
    }
    
    void Update()
    {
        if (currentTarget == null || isPerformingAction)
            return;
        
        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
        
        // Check if the dog reached the target
        if (distance <= 0.6f)
        {
            //print("in target distance");
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
        if (potentialTarget != null)
        {
            if (Random.value < stopProb)
            {
                //Debug.Log("Switching to new target from trigger");
                
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
        currentTarget.StartTargetAction();
    }
    
    private void OnTargetActionComplete()
    {
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
