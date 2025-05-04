using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerThrowController : MonoBehaviour
{
    [SerializeField] private float baseForce = 4f;
    [SerializeField] private Trajectory trajectory;

    [SerializeField] private ThrowableController tempThrowable;

    private Vector2 _aimInput = Vector2.zero;
    private bool _isAiming = false;
    private Vector2 _throwForce;
    private Vector3 _endPoint; 

    private ThrowableController _curThrowableObj = null;

    private void Start()
    {
        _curThrowableObj = tempThrowable;
        _curThrowableObj.StopThrow();
    }

    public void UpdateThrowableObj(ThrowableController obj = null)
    {
        _curThrowableObj = obj;
    }
    
    public void UpdateAimInput(Vector2 input)
    {
        _aimInput = input;
        if (input != Vector2.zero)
        {
            _isAiming = true;
            OnAim();
        }
        else
        {
            _isAiming = false;
        }
    }

    public void OnAim()
    {
        if (_curThrowableObj is null || !_isAiming) return;

        trajectory.Show();
        Vector3 direction = _aimInput.normalized;
        _endPoint = transform.position + direction;
        
        float distance = Vector2.Distance(transform.position, _endPoint);
        _throwForce  = direction * distance * baseForce;
        
        Debug.DrawLine(transform.position, _endPoint);
        trajectory.UpdateDots(_endPoint, _throwForce);
    }

    public void OnThrow()
    {
        if (_curThrowableObj is null || !_isAiming) return;
        trajectory.Hide();
        _curThrowableObj.OnThrow(_throwForce, _endPoint);
    }
}
