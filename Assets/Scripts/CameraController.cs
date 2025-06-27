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
    
    [Header("Zoom Values")]
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minFOV = 5f;
    [SerializeField] private float maxFOV = 7f;
    
    private Coroutine zoomCoroutine;
    
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Debug.LogError("TOO MANY CAMERA CONTROLLERS!");

        FollowPlayer();
    }

    public void FollowPlayer()
    {
        _followPlayerAndDog.enabled = false;
        _followPlayer.enabled = true;
    }

    public void FollowPlayerAndDog()
    {
        _followPlayer.enabled = false;
        _followPlayerAndDog.enabled = true;
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

    public void ZoomOut()
    {
        StartZoom(maxFOV);
    }
    
    public void ZoomIn()
    {
        StartZoom(minFOV);
    }
}
