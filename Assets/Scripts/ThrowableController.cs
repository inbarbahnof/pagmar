using System;
using System.Collections;
using UnityEngine;

public class ThrowableController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector2 _throwDestination;
    private bool _isMoving;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_isMoving && Vector2.Distance(transform.position, _throwDestination) < 0.3f)
        {
            StopThrow();
            _isMoving = false;
        }
    }


    public void OnThrow(Vector2 throwForce, Vector2 destination)
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.AddForce(throwForce, ForceMode2D.Impulse);
        _throwDestination = destination;
        _isMoving = true;
        StartCoroutine(Throwing());
    }

    private IEnumerator Throwing()
    {
        yield return new WaitForSeconds(1f);
        StopThrow();
    }

    public void StopThrow()
    {
        _rb.angularVelocity = 0f;
        _rb.linearVelocity = Vector3.zero;
        _rb.bodyType = RigidbodyType2D.Kinematic;
    }
}