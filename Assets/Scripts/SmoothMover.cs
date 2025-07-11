using System;
using System.Collections;
using Dog;
using UnityEngine;

public class SmoothMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // Default ease
    [SerializeField] private bool _isPlayer;
    [SerializeField] private bool isDog = false;
    [SerializeField] private bool _alive;
    
    private Coroutine moveCoroutine;
    
    [SerializeField] private DogAnimationManager _dog;
    [SerializeField] private PlayerStateManager _player;

    /// <summary>
    /// Moves the GameObject to the target position with easing.
    /// </summary>
    /// <param name="targetPosition">Target world position (2D)</param>
    public float MoveTo(Vector2 targetPosition)
    {
        // Stop any previous movement
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        float distance = Vector2.Distance(transform.position, targetPosition);
        float duration = distance / moveSpeed;
        
        moveCoroutine = StartCoroutine(MoveRoutine(targetPosition, duration));
        return duration;
    }

    private IEnumerator MoveRoutine(Vector2 target, float duration)
    {
        Vector2 start = transform.position;
        float elapsed = 0f;
        
        if(_alive)
        {
            if (_isPlayer) _player.UpdateSmoothMove(true);
            else if (isDog) _dog.UpdateSmoothMove(true, target);
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float easedT = easeCurve.Evaluate(t);
            transform.position = Vector2.Lerp(start, target, easedT);
            yield return null;
        }

        transform.position = target;
        
        if (_alive)
        {
            if (_isPlayer) _player.UpdateSmoothMove(false);
            else if (isDog) _dog.UpdateSmoothMove(false, target);
        }
    }
}