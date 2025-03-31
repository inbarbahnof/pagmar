using System;
using System.Collections;
using UnityEngine;

public class TargetFollow : MonoBehaviour
{
    private float moveSpeed = 5f;
    [SerializeField] private RandomTargetGenerator randomTargetGenerator;
    
    private Transform target;
    private float targetDistance = 3f;
    private float targetHoverTime = 1f; // TODO maybe make this a range
    private bool hover = false;
    
    private Rigidbody2D rb;
    private Vector2 direction;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (target != null)
        {
            Vector2 moveDirection = (target.position - transform.position);
            moveDirection.Normalize();
            direction = moveDirection;
        }
    }

    public void GetNewTarget()
    {
        Target newTarget = randomTargetGenerator.GenerateNewTarget();
        Debug.Log("newTarget: " + newTarget.name);
        target = newTarget.gameObject.transform;
        targetDistance = newTarget.GetDistance();
        StartCoroutine(HoverCoroutine());
    }

    private IEnumerator HoverCoroutine()
    {
        hover = true;
        yield return new WaitForSeconds(targetHoverTime);
        hover = false;
    }
    
    private void FixedUpdate()
    {
        // there is a target, and I am too far from it
        if (target != null && Vector3.Distance(transform.position, target.position) > targetDistance)
        {
            MoveCharacter(direction);
        }
        // there is not a target, or I am close to the target, and I am not hovering
        else if (!hover)
        {
            GetNewTarget();
        }
    }

    private void MoveCharacter(Vector2 moveDirection)
    {
        rb.MovePosition((Vector2)transform.position + (moveDirection * (moveSpeed * Time.deltaTime)));
    }
}
