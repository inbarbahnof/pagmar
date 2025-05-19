using UnityEngine;

public class AimControl : MonoBehaviour
{
    [SerializeField] private float baseForce = 4f;
    [SerializeField] private Trajectory trajectory;
    
    private Vector2 _aimInput = Vector2.zero;
    private bool _isAiming = false;
    private Vector2 _throwForce;

    public bool IsAiming => _isAiming;
    
    // throw input
    private Vector2 _endPoint;
    private Vector2 startPoint;
    private float throwSpeed = 10f;
    public float arcHeight = 2f;
    
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
            trajectory.Hide();
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

        //_stateManager.SetState(PlayerState.Aim);
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
    
    public void HideTrajectory()
    {
        trajectory.Hide();
        UpdateAimInput(Vector2.zero);
    }

    public ThrowInput GetCurThrowInput()
    {
        return new ThrowInput(startPoint, _endPoint, throwSpeed, arcHeight);
    }
}