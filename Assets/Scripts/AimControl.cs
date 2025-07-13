using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AimControl : MonoBehaviour
{
    [SerializeField] private GameObject validTrajectoryTarget;
    [SerializeField] private GameObject invalidTrajectoryTarget;
    [SerializeField] private float throwDistance = 5f;
    [SerializeField] private float arcHeight = 2.5f;
    [SerializeField] private float targetRadius = 1f;
    
    private NavMeshHit _navMeshHit;
    private GameObject _curTarget;
    private bool _curAimValid = false;
    
    private Vector2 _aimInput = Vector2.zero;
    private bool _isAiming = false;
    private Vector2 _throwForce;

    public bool IsAiming => _isAiming;

    public bool CurAimValid => _curAimValid;
    
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
            _curTarget.SetActive(false);
            return;
        }

        _aimInput = input;
        _isAiming = true;
    }

    private void UpdateTarget(bool isValid)
    {
        _curAimValid = isValid;
        validTrajectoryTarget.SetActive(isValid);
        invalidTrajectoryTarget.SetActive(!isValid);
        _curTarget = isValid ? validTrajectoryTarget : invalidTrajectoryTarget;
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

        if (_curTarget is null || !_curTarget.activeInHierarchy)
        {
            UpdateTarget(true);
        }
        startPoint = transform.position;
        _endPoint = startPoint + input.normalized * throwDistance;
        Vector2 validEndPoint = GetValidTarget(_endPoint);
        if (validEndPoint != Vector2.zero)
        {
            _endPoint = validEndPoint;
            UpdateTarget(true);
        }
        else
        {
            UpdateTarget(false);
        }

        Debug.DrawLine(transform.position, _endPoint, Color.green);
        validTrajectoryTarget.transform.position = _endPoint;
        invalidTrajectoryTarget.transform.position = _endPoint;
    }
    
    public void HideTrajectory()
    {
        validTrajectoryTarget.SetActive(false);
        invalidTrajectoryTarget.SetActive(false);
        _curTarget = validTrajectoryTarget;
        UpdateAimInput(Vector2.zero);
    }

    public ThrowInput GetCurThrowInput()
    {
        return new ThrowInput(startPoint, _endPoint, throwSpeed, arcHeight);
    }

    private Vector2 GetValidTarget(Vector2 target)
    {
        if (NavMesh.SamplePosition(target, out _navMeshHit, targetRadius, NavMesh.AllAreas))
        {
            return _navMeshHit.position;
        }
        return Vector2.zero;
    }
}