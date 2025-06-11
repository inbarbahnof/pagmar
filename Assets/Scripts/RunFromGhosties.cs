using System;
using Dog;
using UnityEngine;

public class RunFromGhosties : MonoBehaviour
{
    [SerializeField] private bool _isRunning;
    [SerializeField] private PlayerMove _playerMove;
    private bool _didDogPass;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Dog"))
        {
            _didDogPass = true;
            print("dog passed");
        }
        else if (other.CompareTag("Player"))
        {
            if (_isRunning)
            {
                print("running");
                
                if (!_didDogPass) 
                    _playerMove.SetCanMove(false);
                else 
                    StartsRunning();
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
