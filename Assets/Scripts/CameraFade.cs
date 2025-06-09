using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraFade : MonoBehaviour
{
    [SerializeField] private Image fade;
    public Color startColor = new Color(0, 0, 0, 0);
    public Color endColor = new Color(0, 0, 0, 1);

    private Coroutine _fadeOutCoroutine;

    public void FadeOutOverTime(bool reverse = false)
    {
        float value = Mathf.Clamp01(0.75f);

        if (_fadeOutCoroutine != null)
            StopCoroutine(_fadeOutCoroutine);

        _fadeOutCoroutine = StartCoroutine(LerpColor(value, reverse));
    }

    private IEnumerator LerpColor(float targetValue, bool reverse)
    {
        float duration = 3f;
        float time = 0f;

        Color fromColor = reverse
            ? Color.Lerp(startColor, endColor, targetValue)
            : fade.color;

        Color toColor = reverse
            ? startColor
            : Color.Lerp(startColor, endColor, targetValue);

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / duration;
            fade.color = Color.Lerp(fromColor, toColor, t);
            yield return null;
        }

        fade.color = toColor;
        _fadeOutCoroutine = null;
    }
}