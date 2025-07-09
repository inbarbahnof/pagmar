using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Fades Image.colour in/out between black and transparent,
/// where start colour is transparent and end colour is black.
/// </summary>
public class CameraFade : MonoBehaviour, IObjectFader
{
    [SerializeField] private Image fade;
    [SerializeField] private float duration = 3f;
    private readonly Color _startColor = new Color(0, 0, 0, 0);
    private readonly Color _endColor = new Color(0, 0, 0, 1);
    [SerializeField] private float delayOnFadeIn = 2f;

    private Coroutine _fadeOutCoroutine;

    public float Duration => duration;

    public void FadeOutOverTime(bool reverse = false)
    {
        if (!fade) return;
        if (_fadeOutCoroutine != null)
            StopCoroutine(_fadeOutCoroutine);

        _fadeOutCoroutine = StartCoroutine(LerpColor(reverse));
    }

    private IEnumerator LerpColor(bool reverse)
    {
        if (reverse && delayOnFadeIn > 0) yield return new WaitForSecondsRealtime(delayOnFadeIn);
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