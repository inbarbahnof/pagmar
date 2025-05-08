using System;
using System.Collections;
using Targets;
using UnityEngine;

public class PlayerThrowController : MonoBehaviour
{
    [SerializeField] private float baseForce = 4f;
    [SerializeField] private Trajectory trajectory;
    [SerializeField] private GameObject curThrowableObj;
    
    private Vector2 _aimInput = Vector2.zero;
    private bool _isAiming = false;
    private Vector2 _throwForce;
    private Vector2 _endPoint;
    private Vector2 startPoint;
    
    private PlayerStateManager _stateManager;

    private float throwSpeed = 10f;
    public float arcHeight = 2f;
    
    // aim feel
    private const float inputDeadzone = 0.05f;
    private Vector2 _smoothedAimInput;
    [SerializeField] private float aimSmoothSpeed = 10f; // Higher = snappier

    private void Start()
    {
        _stateManager = GetComponent<PlayerStateManager>();
        _smoothedAimInput = Vector2.zero;
    }

    public void UpdateThrowableObj(GameObject obj = null)
    {
        curThrowableObj = obj;
    }
    
    public void UpdateAimInput(Vector2 input)
    {
        // _aimInput = input;
        // _isAiming = input != Vector2.zero;
        //
        // if (!_isAiming)
        // {
        //     trajectory.Hide();
        // }
        
        if (input.magnitude < inputDeadzone)
        {
            _aimInput = Vector2.zero;
            _isAiming = false;
            trajectory.Hide();
            return;
        }

        _aimInput = input;
        _isAiming = true;
    }

    private void Update()
    {
        if (_isAiming && curThrowableObj is not null)
        {
            _smoothedAimInput = Vector2.Lerp(_smoothedAimInput, _aimInput, Time.deltaTime * aimSmoothSpeed);
            OnAim(_smoothedAimInput);
        }
    }

    public void OnAim(Vector2 input)
    {
        if (curThrowableObj is null || !_isAiming) return;

        trajectory.Show();
        startPoint = transform.position;
        
        float inputStrength = Mathf.Clamp01(input.magnitude);
        float distance = Mathf.Lerp(2f, 6f, inputStrength);     // Scale distance
        float arc = Mathf.Lerp(1f, 3f, inputStrength);          // Scale arc height
        arcHeight = arc; // Store for use in Throw()
        
        _endPoint = startPoint + input.normalized * distance;
        Vector2 controlPoint = (startPoint + _endPoint) * 0.5f;
        controlPoint.y += arcHeight;
        
        Debug.DrawLine(transform.position, _endPoint, Color.green);
        trajectory.UpdateDots(startPoint, _endPoint, controlPoint);
    }
    
    public void OnThrow()
    {
        if (curThrowableObj is null || !_isAiming) return;
        
        _stateManager.SetState(PlayerState.Throw);
        TargetGenerator.instance.SetStickTarget(curThrowableObj.GetComponent<Target>());
        StartCoroutine(Throw());
        trajectory.Hide();
    }

    private IEnumerator Throw()
    {
        float duration = Vector2.Distance(startPoint, _endPoint) / throwSpeed;
        float elapsed = 0f;
        
        // Recalculate control point (identical to OnAim)
        Vector2 controlPoint = (startPoint + _endPoint) * 0.5f;
        controlPoint.y += arcHeight;
        
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            Vector2 pos = Evaluate(t, controlPoint);
            
            curThrowableObj.transform.position = pos;
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        curThrowableObj.transform.position = _endPoint;
        //UpdateThrowableObj();

    }
    
    private Vector2 Evaluate(float t, Vector2 controlPoint)
    {
        Vector2 sc = Vector2.Lerp(startPoint, controlPoint, t);
        Vector2 ce = Vector2.Lerp(controlPoint, _endPoint, t);
        return Vector2.Lerp(sc, ce, t);
    }
}
