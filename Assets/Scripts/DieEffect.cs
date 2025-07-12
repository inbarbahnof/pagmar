using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DieEffect : MonoBehaviour
{
    [SerializeField] private float _waitToStop = 1;
    [SerializeField] private CameraFade _fader;
    [SerializeField] private VisualEffect[] _vfx;

    public void PlayEffect()
    {
        _fader.FadeOutOverTime();
        gameObject.SetActive(true);

        StartCoroutine(WaitToStopVFX());
    }

    private IEnumerator WaitToStopVFX()
    {
        yield return new WaitForSeconds(_waitToStop);

        foreach (var vfx in _vfx)
        {
            vfx.Stop();
        }

        _fader.FadeOutOverTime(true);
        yield return new WaitForSeconds(3f);

        gameObject.SetActive(false);
        foreach (var vfx in _vfx)
        {
            vfx.Play();
        }
    }
}
