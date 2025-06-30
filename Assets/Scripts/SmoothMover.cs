using System;
using System.Collections;
using Dog;
using UnityEngine;

public class SmoothMover : MonoBehaviour
{
    [SerializeField] private float moveDuration = 0.5f; // Seconds
    [SerializeField] private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // Default ease
    [SerializeField] private bool _isPlayer = true;
    [SerializeField] private bool _alive;
    
    private Coroutine moveCoroutine;
    
    private DogAnimationManager _dog;
    private PlayerStateManager _player;

    private void Start()
    {
        if (_alive)
        {
            if (_isPlayer) _player = GetComponent<PlayerStateManager>();
            else _dog = GetComponent<DogAnimationManager>();
        }
    }

    /// <summary>
    /// Moves the GameObject to the target position with easing.
    /// </summary>
    /// <param name="targetPosition">Target world position (2D)</param>
    public float MoveTo(Vector2 targetPosition, float duration = 0.5f)
    {
        // Stop any previous movement
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveRoutine(targetPosition, duration));
        return moveDuration;
    }

    private IEnumerator MoveRoutine(Vector2 target, float duration)
    {
        Vector2 start = transform.position;
        float elapsed = 0f;
        
        if(_alive)
        {
            if (_isPlayer) _player.UpdateSmoothMove(true);
            else _dog.UpdateSmoothMove(true, target);
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float easedT = easeCurve.Evaluate(t);
            transform.position = Vector2.Lerp(start, target, easedT);
            yield return null;
        }

        transform.position = target; // Ensure it's exact
        
        if(_alive)
        {
            if (_isPlayer) _player.UpdateSmoothMove(false);
            else _dog.UpdateSmoothMove(false, target);
        }
    }
}