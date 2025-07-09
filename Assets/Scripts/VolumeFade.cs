using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Fades Volume.weight in/out between 0 and 1,
/// where start weight is obj og weight and end weight is the opposite option.
/// </summary>
public class VolumeFade : MonoBehaviour, IObjectFader
{
    [SerializeField] private float duration = 4f;
    [SerializeField] private Volume volume;
    
    private float _startVal;
    private float _endVal;

    private Coroutine _fadeOutCoroutine;

    public float Duration => duration;

    private void Start()
    {
        _startVal = volume.weight;
        _endVal = volume.weight == 0 ? 1 : 0;
    }

    public void FadeOutOverTime(bool reverse = false)
    {
        if (!volume) return;
        if (_fadeOutCoroutine != null)
            StopCoroutine(_fadeOutCoroutine);

        _fadeOutCoroutine = StartCoroutine(LerpOutColor(reverse));
    }

    private IEnumerator LerpOutColor(bool reverse)
    {
        float time = 0f;

        float fromVal = reverse ? _endVal : _startVal;
        float toVal   = reverse ? _startVal : _endVal;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / duration);
            //float easedT = Mathf.SmoothStep(0f, 1f, t); // Smooth easing
            float easedT = EaseInOutCubic(t);
            volume.weight = Mathf.Lerp(fromVal, toVal, easedT);
            yield return null;
        }

        volume.weight = toVal;
        _fadeOutCoroutine = null;
    }
    
    private float EaseInOutCubic(float t)
    {
        return t < 0.5f
            ? 4f * t * t * t
            : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
    }
}