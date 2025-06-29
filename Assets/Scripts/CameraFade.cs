using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraFade : MonoBehaviour
{
    [SerializeField] private Image fade;
    [SerializeField] private float duration = 3f;
    private readonly Color _startColor = new Color(0, 0, 0, 0);
    private readonly Color _endColor = new Color(0, 0, 0, 1);

    private Coroutine _fadeOutCoroutine;

    public float Duration => duration;

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.LeftShift))
    //     {
    //         FadeOutOverTime(true);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.RightShift))
    //     {
    //         FadeOutOverTime(false);
    //     }
    // }

    public void FadeOutOverTime(bool reverse = false)
    {
        if (!fade) return;
        if (_fadeOutCoroutine != null)
            StopCoroutine(_fadeOutCoroutine);

        _fadeOutCoroutine = StartCoroutine(LerpColor(reverse));
    }

    private IEnumerator LerpColor(bool reverse)
    {
        if (reverse) yield return new WaitForSecondsRealtime(2f);
        float time = 0f;

        Color fromColor = reverse ? _endColor : _startColor;
        Color toColor   = reverse ? _startColor : _endColor;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / duration);
            float easedT = Mathf.SmoothStep(0f, 1f, t); // Smooth easing
            fade.color = Color.Lerp(fromColor, toColor, easedT);
            yield return null;
        }

        fade.color = toColor;
        _fadeOutCoroutine = null;
    }
}