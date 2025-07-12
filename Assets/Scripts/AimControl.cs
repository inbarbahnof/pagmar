using System.Collections;
using UnityEngine;

public class AimControl : MonoBehaviour
{
    [SerializeField] private GameObject trajectoryTarget;
    [SerializeField] private float throwDistance = 5f;
    [SerializeField] private float arcHeight = 2.5f;
    
    private Vector2 _aimInput = Vector2.zero;
    private bool _isAiming = false;
    private Vector2 _throwForce;

    public bool IsAiming => _isAiming;
    
    // throw input
    private Vector2 _endPoint;
    private Vector2 startPoint;
    private float throwSpeed = 10f;
    
    // aim feel
    private const float inputDeadzone = 0.05f;
    private Vector2 _smoothedAimInput = Vector2.zero;
    [SerializeField] private float aimSmoothSpeed = 10f; // Higher = snappier
    
    public void UpdateAimInput(Vector2 input)
    {
        if (input.magnitude < inputDeadzone)
        {
            _aimInput = Vector2.zero;
            _isAiming = false;
            trajectoryTarget.SetActive(false);
            return;
        }

        _aimInput = input;
        _isAiming = true;
    }
    
    private void Update()
    {
        if (_isAiming)
        {
            _smoothedAimInput = Vector2.Lerp(_smoothedAimInput, _aimInput, Time.deltaTime * aimSmoothSpeed);
            OnAim(_smoothedAimInput);
        }
    }
    
    public void OnAim(Vector2 input)
    {
        if (!_isAiming) return;

        trajectoryTarget.SetActive(true);
        startPoint = transform.position;

        _endPoint = startPoint + input.normalized * throwDistance;

        Vector2 direction = input.normalized;
        Vector2 midpoint = (startPoint + _endPoint) * 0.5f;

        Vector2 controlPoint = midpoint + Vector2.up * arcHeight;
        
        Debug.DrawLine(transform.position, _endPoint, Color.green);
        trajectoryTarget.transform.position = _endPoint;
    }
    
    public void HideTrajectory()
    {
        trajectoryTarget.SetActive(false);
        UpdateAimInput(Vector2.zero);
    }

    public ThrowInput GetCurThrowInput()
    {
        return new ThrowInput(startPoint, _endPoint, throwSpeed, arcHeight);
    }
}