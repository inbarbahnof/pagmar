using System;
using System.Collections;
using Audio.FMOD;
using UnityEngine;
using UnityEngine.VFX;

public class DieEffect : MonoBehaviour
{
    [SerializeField] private float _waitToStop = 1;
    [SerializeField] private CameraFade _fader;
    [SerializeField] private VisualEffect[] _vfx;

    public void PlayEffect()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.PlayerDeath,
            transform.position, true);
        StartCoroutine(WaitToStopVFX());
    }

    private IEnumerator WaitToStopVFX()
    {
        _fader.FadeOutOverTime();

        foreach (var vfx in _vfx)
        {
            vfx.Play();
        }
        
        yield return new WaitForSeconds(_waitToStop);

        foreach (var vfx in _vfx)
        {
            vfx.Stop();
        }

        yield return new WaitForSeconds(1f);
        _fader.FadeOutOverTime(true);
    }
}
