using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    
    [SerializeField] private CinemachineCamera _followPlayer;
    [SerializeField] private CinemachineCamera _followPlayerAndDog;
    [SerializeField] private CinemachineCamera[] cutsceneCams;
    [SerializeField] private CinemachineCamera _followPlayerPan;
    
    [Header("Zoom Values")]
    [SerializeField] private float minFOV = 5f;
    [SerializeField] private float maxFOV = 7f;
    [SerializeField] private float extraZoomIn = 3f;

    [Header("Effects")]
    [SerializeField] private Transform _ghostieEffect;
    [SerializeField] private Transform _ghostEffect;
    
    private float zoomSpeed = 2f;
    private Coroutine zoomCoroutine;
    
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Debug.LogError("TOO MANY CAMERA CONTROLLERS!");

        StartCoroutine(FollowPlayerCoroutine());
    }

    private IEnumerator FollowPlayerCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        if (GameManager.instance.Chapter != 0) FollowPlayer();
    }

    public void UpdatePriorities(CinemachineCamera priorityCam)
    {
        priorityCam.Priority = 1;
        priorityCam.enabled = true;
        if (priorityCam != _followPlayerAndDog)
        {
            _followPlayerAndDog.Priority = 0;
            _followPlayerAndDog.enabled = false;
        }

        if (priorityCam != _followPlayer)
        {
            _followPlayer.Priority = 0;
            _followPlayer.enabled = false;
        }
    }

    public void FollowPlayer()
    {
        _followPlayerAndDog.enabled = false;
        _followPlayer.enabled = true;
        SwitchEffectParent(_followPlayer.transform);

        if (_followPlayerPan != null) _followPlayerPan.enabled = false;
    }

    public void FollowPlayerPan()
    {
        _followPlayerAndDog.enabled = false;
        _followPlayer.enabled = false;
        _followPlayerPan.enabled = true;
        
        SwitchEffectParent(_followPlayerPan.transform);
    }

    public void FollowPlayerAndDog()
    {
        _followPlayer.enabled = false;
        _followPlayerAndDog.enabled = true;
        SwitchEffectParent(_followPlayerAndDog.transform);
        
        if (_followPlayerPan != null) _followPlayerPan.enabled = false;
    }

    public void DisableCutsceneCams()
    {
        if (cutsceneCams.Length == 0) return;
        foreach (var cam in cutsceneCams)
        {
            if (cam is not null) cam.enabled = false;
        }
    }
    
    private CinemachineCamera GetActiveCamera()
    {
        if (_followPlayer.enabled) return _followPlayer;
        if (_followPlayerAndDog.enabled) return _followPlayerAndDog;
        return null;
    }
    
    private void StartZoom(float targetFOV)
    {
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);
        zoomCoroutine = StartCoroutine(SmoothZoom(targetFOV));
    }

    private IEnumerator SmoothZoom(float targetSize)
    {
        var cam = GetActiveCamera();
        if (cam == null) yield break;

        while (Mathf.Abs(cam.Lens.OrthographicSize - targetSize) > 0.01f)
        {
            cam.Lens.OrthographicSize = Mathf.Lerp(cam.Lens.OrthographicSize, targetSize, Time.deltaTime * zoomSpeed);
            yield return null;
        }

        cam.Lens.OrthographicSize = targetSize; // Snap to exact value at the end
    }
    
    void SwitchEffectParent(Transform newFollowTarget)
    {
        _ghostieEffect.SetParent(newFollowTarget);
        _ghostEffect.SetParent(newFollowTarget);
    }

    public void ZoomOut(float speed = 2)
    {
        StartZoom(maxFOV);
        zoomSpeed = speed;
    }
    
    public void ZoomIn(float speed = 2)
    {
        StartZoom(minFOV);
        zoomSpeed = speed;
    }

    public void ExtraZoomIn(float speed = 2)
    {
        StartZoom(extraZoomIn);
        zoomSpeed = speed;
    }
}
