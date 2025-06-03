using System;
using UnityEngine;

public class ZoomCamera : MonoBehaviour
{
    [SerializeField] private bool _isZoomIn = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(_isZoomIn) CameraController.instance.ZoomIn();
            else CameraController.instance.ZoomOut();
        }
    }
}
