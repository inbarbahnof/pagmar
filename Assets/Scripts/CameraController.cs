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
    [SerializeField] private CinemachineCamera _panCameraEnd;
    [SerializeField] private CinemachineCamera _cageCamera;
    
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

    public void DeprioritizeCam(CinemachineCamera cam)
    {
        cam.Priority = 0;
    }

    public void FollowPlayer()
    {
        _followPlayerAndDog.enabled = false;
        _followPlayer.enabled = true;

        if (_followPlayerPan != null) _followPlayerPan.enabled = false;
        if (_panCameraEnd != null) _panCameraEnd.enabled = false;
        
        SwitchEffectParent(_followPlayer.transform);
    }

    public void FollowPlayerPan()
    {
        _followPlayerAndDog.enabled = false;
        _followPlayer.enabled = false;
        _followPlayerPan.enabled = true;
        
        if (_panCameraEnd != null) _panCameraEnd.enabled = false;
        SwitchEffectParent(_followPlayerPan.transform);
    }

    public void PanCameraEnd()
    {
        _followPlayerAndDog.enabled = false;
        _followPlayer.enabled = false;
        _followPlayerPan.enabled = false;
        
        _panCameraEnd.enabled = true;
    }

    public void FollowPlayerAndDog()
    {
        _followPlayer.enabled = false;
        _followPlayerAndDog.enabled = true;

        if (_followPlayerPan != null) _followPlayerPan.enabled = false;
        if (_panCameraEnd != null) _panCameraEnd.enabled = false;
        
        SwitchEffectParent(_followPlayerAndDog.transform);
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
        if (_followPlayerPan != null && _followPlayerPan.enabled) return _followPlayerPan;
        if (_panCameraEnd != null && _panCameraEnd.enabled) return _panCameraEnd;
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
    
    private void SwitchEffectParent(Transform newFollowTarget)
    {
        // Step 1: Save current *world* position and rotation
        Vector3 ghostieWorldPos = _ghostieEffect.localPosition;
        Vector3 ghostWorldPos = _ghostEffect.localPosition;

        // Step 2: Set the new parent
        _ghostieEffect.SetParent(newFollowTarget);
        _ghostEffect.SetParent(newFollowTarget);

        // Step 3: Re-apply saved *world* position and rotation
        _ghostieEffect.localPosition = ghostieWorldPos;
        _ghostEffect.localPosition = ghostWorldPos;
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

    public void ExtraZoomOut(bool offset ,float speed = 2)
    {
        if (offset)
        {
            _cageCamera.Priority = 1;
        }
        else
        {
            StartZoom(9);
            zoomSpeed = speed;
        }
        
        StartCoroutine(WaitToNormalZoom(offset));
    }
    
    private IEnumerator WaitToNormalZoom(bool offset)
    {
        if (offset)
        {
            yield return new WaitForSeconds(2.5f);
            _cageCamera.Priority = 0;
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            ZoomOut();
        }
    }

    public void ExtraZoomIn(float speed = 2)
    {
        StartZoom(extraZoomIn);
        zoomSpeed = speed;
    }
}
