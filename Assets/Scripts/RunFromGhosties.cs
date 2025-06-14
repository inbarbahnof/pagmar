using System;
using Dog;
using UnityEngine;

public class RunFromGhosties : MonoBehaviour
{
    [SerializeField] private bool _isRunning;
    [SerializeField] private PlayerMove _playerMove;
    private bool _didDogPass;
    private bool _playerWaiting;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Dog"))
        {
            _didDogPass = true;
            print("dog passed");
            
            if (_playerWaiting) StartsRunning();
        }
        else if (other.CompareTag("Player"))
        {
            if (_isRunning)
            {
                if (!_didDogPass)
                {
                    print("waiting");
                    _playerWaiting = true;
                    _playerMove.SetCanMove(false);
                }
                else
                {
                    print("running");
                    StartsRunning();
                }
            }
            else
            {
                _playerMove.StopAutoRun();
                CameraController.instance.ZoomIn();
            }
        }
    }

    public void StartsRunning()
    {
        _didDogPass = true;
        
        _playerMove.StartAutoRunWithVerticalControl();
        CameraController.instance.ZoomOut();
    }
}
