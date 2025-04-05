using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private List<Target> targets;
    
    private NavMeshAgent agent;
    private Target currentTarget;
    private float targetDistance;
    private bool isPerformingAction = false;
    private bool isGoingToTarget = true;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
        GetNewTarget();
    }
    
    void Update()
    {
        if (currentTarget == null || isPerformingAction)
            return;
        
        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
        
        // Check if the dog reached the target
        if (distance <= targetDistance)
        {
            print("in target distance");
            agent.isStopped = true;
            StartTargetAction();
        }
        else if (isGoingToTarget)
        {
            agent.isStopped = false;
            agent.SetDestination(currentTarget.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Target potentialTarget = other.GetComponent<Target>();
        if (potentialTarget != null)
        {
            if (Random.value < 0.7f) // 50% chance to switch to this target
            {
                Debug.Log("Switching to new target from trigger");
            
                // Unsubscribe from old target
                if (currentTarget != null)
                    currentTarget.OnTargetActionComplete -= OnTargetActionComplete;

                currentTarget = potentialTarget;
                targetDistance = currentTarget.GetDistance();
                currentTarget.OnTargetActionComplete += OnTargetActionComplete;
                isPerformingAction = false;
                isGoingToTarget = true;
            }
        }
    }

    private void StartTargetAction()
    {
        isPerformingAction = true;
        isGoingToTarget = false;
        currentTarget.StartTargetAction();
    }

    private void GetNewTarget()
    {
        print("in get new target");
        isGoingToTarget = true;
        currentTarget = targets[Random.Range(0, targets.Count)];
        targetDistance = currentTarget.GetDistance();
        currentTarget.OnTargetActionComplete += OnTargetActionComplete;
    }
    
    private void OnTargetActionComplete()
    {
        Debug.Log("Target action completed111");
        isPerformingAction = false;
        currentTarget.OnTargetActionComplete -= OnTargetActionComplete;
        GetNewTarget();
    }
}
