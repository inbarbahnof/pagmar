using System.Collections;
using UnityEngine;

public class SmoothMover : MonoBehaviour
{
    [SerializeField] private float moveDuration = 0.5f; // Seconds
    [SerializeField] private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // Default ease

    private Coroutine moveCoroutine;

    /// <summary>
    /// Moves the GameObject to the target position with easing.
    /// </summary>
    /// <param name="targetPosition">Target world position (2D)</param>
    public float MoveTo(Vector2 targetPosition)
    {
        // Stop any previous movement
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveRoutine(targetPosition));
        return moveDuration;
    }

    private IEnumerator MoveRoutine(Vector2 target)
    {
        Vector2 start = transform.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            float easedT = easeCurve.Evaluate(t);
            transform.position = Vector2.Lerp(start, target, easedT);
            yield return null;
        }

        transform.position = target; // Ensure it's exact
    }
}