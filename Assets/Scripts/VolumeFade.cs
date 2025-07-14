
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeFade : MonoBehaviour, IObjectFader
{
    [SerializeField] private float duration = 4f;
    [SerializeField] private Volume volume;

    private Coroutine _fadeOutCoroutine;

    public float Duration => duration;

    public float FadeOutOverTime(bool reverse = false)
    {
        if (!volume || !volume.gameObject.activeInHierarchy) return 0;

        if (_fadeOutCoroutine != null)
            StopCoroutine(_fadeOutCoroutine);

        // Determine start and end based on current volume state
        float fromVal = volume.weight;
        float toVal = reverse
            ? (fromVal == 0f ? 1f : 0f)  // go opposite of current
            : (fromVal == 1f ? 0f : 1f); // default: fade to opposite

        _fadeOutCoroutine = StartCoroutine(LerpVolume(fromVal, toVal));
        return duration;
    }

    private IEnumerator LerpVolume(float fromVal, float toVal)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / duration);
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