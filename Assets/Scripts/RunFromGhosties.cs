using System;
using Dog;
using UnityEngine;

public class RunFromGhosties : MonoBehaviour
{
    [SerializeField] private bool _isRunning;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove player = other.GetComponent<PlayerMove>();
            if (_isRunning)
            {
                player.StartAutoRunWithVerticalControl();
                CameraController.instance.ZoomOut();
            }
            else
            {
                player.StopAutoRun();
                CameraController.instance.ZoomIn();
            }
        }
    }
}
