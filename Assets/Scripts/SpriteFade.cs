using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Fades sprite colour in/out between opaque and transparent,
/// where opaque colour is sprite og colour.
/// </summary>
public class SpriteFade : MonoBehaviour, IObjectFader
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private Color _opaqueColor;
    private Color _transparentColor;

    private Coroutine _fadeOutCoroutine;

    public float Duration => duration;

    private void Start()
    {
        _opaqueColor = spriteRenderer.color;
        _opaqueColor.a = 1;
        _transparentColor = spriteRenderer.color;
        _transparentColor.a = 0;
    }

    public void FadeOutOverTime(bool reverse = false)
    {
        if (!spriteRenderer || _opaqueColor == default || _transparentColor == default) return;
        if (_fadeOutCoroutine != null)
            StopCoroutine(_fadeOutCoroutine);

        _fadeOutCoroutine = StartCoroutine(LerpOutColor(reverse));
    }

    private IEnumerator LerpOutColor(bool reverse)
    {
        float time = 0f;

        Color fromColor = reverse ? _transparentColor : _opaqueColor;
        Color toColor   = reverse ? _opaqueColor : _transparentColor;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / duration);
            float easedT = Mathf.SmoothStep(0f, 1f, t); // Smooth easing
            spriteRenderer.color = Color.Lerp(fromColor, toColor, easedT);
            yield return null;
        }

        spriteRenderer.color = toColor;
        _fadeOutCoroutine = null;
    }
}