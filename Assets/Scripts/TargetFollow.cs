using System;
using System.Collections;
using UnityEngine;

public class TargetFollow : MonoBehaviour
{
    [SerializeField] private RandomTargetGenerator randomTargetGenerator;
 
    private float moveSpeed = 5f;
    private float targetDistance = 3f;
    private float targetHoverTime = 1f; // TODO maybe make this a range
    
    private Rigidbody2D rb;
    private Vector2 direction;
    
    private bool isPerformingAction = false;
    private Transform target;
    private Target currentTarget;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetNewTarget();
    }
    
    void Update()
    {
        print("in update");
        if (target != null)
        {
            print("in target");
            Vector2 moveDirection = (target.position - transform.position);
            moveDirection.Normalize();
            direction = moveDirection;
        }
    }
    
    public void GetNewTarget()
    {
        currentTarget = randomTargetGenerator.GenerateNewTarget();
        Debug.Log("New target: " + currentTarget.name);

        target = currentTarget.transform;
        targetDistance = currentTarget.GetDistance();
        currentTarget.OnTargetActionComplete += OnTargetActionComplete; // Subscribe to event
    }
    
    private void FixedUpdate()
    {
        if (target != null && !isPerformingAction)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            
            if (distance > targetDistance)
            {
                MoveCharacter(direction);
            }
            else
            {
                StartTargetAction();
            }
        }
    }

    private void MoveCharacter(Vector2 moveDirection)
    {
        rb.MovePosition((Vector2)transform.position + (moveDirection * (moveSpeed * Time.deltaTime)));
    }
    
    private void StartTargetAction()
    {
        if (currentTarget != null)
        {
            isPerformingAction = true;
            currentTarget.StartTargetAction();
        }
    }

    private void OnTargetActionComplete()
    {
        Debug.Log("Target action completed");
        isPerformingAction = false;
        currentTarget.OnTargetActionComplete -= OnTargetActionComplete; // Unsubscribe
        GetNewTarget();
    }
}
