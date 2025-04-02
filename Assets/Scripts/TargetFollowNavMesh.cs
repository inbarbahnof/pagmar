using UnityEngine;
using UnityEngine.AI;

public class TargetFollowNavMesh : MonoBehaviour
{
    [SerializeField] private RandomTargetGenerator randomTargetGenerator;
    
    private NavMeshAgent agent;
    private Target currentTarget;
    private float targetDistance;
    private bool isPerformingAction = false;
    private bool isGoingToTarget = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // Disable rotation for 2D movement
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
            StartTargetAction();
        }
        else if (isGoingToTarget)
        {
            agent.SetDestination(currentTarget.transform.position);
        }
    }

    private void GetNewTarget()
    {
        currentTarget = randomTargetGenerator.GenerateNewTarget();
        if (currentTarget != null)
        {
            isGoingToTarget = true;
            Debug.Log("New target: " + currentTarget.name);
            targetDistance = currentTarget.GetDistance();
            currentTarget.OnTargetActionComplete += OnTargetActionComplete;
        }
    }

    private void StartTargetAction()
    {
        if (currentTarget != null)
        {
            isPerformingAction = true;
            isGoingToTarget = false;
            currentTarget.StartTargetAction();
        }
    }

    private void OnTargetActionComplete()
    {
        Debug.Log("Target action completed");
        isPerformingAction = false;
        currentTarget.OnTargetActionComplete -= OnTargetActionComplete;
        GetNewTarget();
    }
}