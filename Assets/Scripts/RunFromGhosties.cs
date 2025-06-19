using System;
using System.Collections;
using Dog;
using UnityEngine;

public class RunFromGhosties : MonoBehaviour
{
    [SerializeField] private bool _isRunning;
    [SerializeField] private PlayerMove _playerMove;
    
    private bool _didDogPass;
    private bool _playerWaiting;
    private Coroutine _slowMotion;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Dog"))
        {
            _didDogPass = true;
            // print("dog passed");
            
            if (_playerWaiting) StartsRunning();
        }
        else if (other.CompareTag("Player"))
        {
            if (_isRunning)
            {
                if (!_didDogPass)
                {
                    // print("waiting");
                    _playerWaiting = true;
                    _playerMove.SetCanMove(false);
                }
                else
                {
                    // print("running");
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
        
        if (_slowMotion == null) _slowMotion = StartCoroutine(SlowMotion());
    }
    
    private IEnumerator SlowMotion()
    {
        Time.timeScale = 0.7f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(12f);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
