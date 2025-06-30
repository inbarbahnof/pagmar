using System;
using System.Collections;
using Audio.FMOD;
using Dog;
using Targets;
using UnityEngine;

public class RunFromGhosties : MonoBehaviour
{
    [SerializeField] private bool _isRunning;
    [SerializeField] private PlayerMove _playerMove;
    [SerializeField] private DogActionManager dog;
    [SerializeField] private WantFoodTarget _wantFoodTarget;
    
    // private bool _didDogPass;
    // private bool _playerWaiting;
    private Coroutine _slowMotion;

    public void StartsRunning()
    {
        if (_isRunning)
        {
            CameraController.instance.ZoomOut();
            StartCoroutine(StartRunCorutine());

            if (_slowMotion == null) _slowMotion = StartCoroutine(SlowMotion());
        }
    }

    public void PlayerStopRunning()
    {
        _playerMove.StopAutoRun();
        CameraController.instance.ZoomIn();
    }

    private IEnumerator StartRunCorutine()
    {
        DogRun();
        yield return new WaitForSeconds(1.5f);
        _playerMove.StartAutoRunWithVerticalControl();

        AudioManager.Instance.SetFloatParameter(AudioManager.Instance.musicInstance,
            "Ending Run", 1, false);
    }
    
    private void DogRun()
    {
        TargetGenerator.instance.SetWantFoodTarget(_wantFoodTarget);
        dog.SetWantsFood(true);
        if (_isRunning) dog.Running(true);
    }
    
    private IEnumerator SlowMotion()
    {
        Time.timeScale = 0.7f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(15f);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
    
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Dog"))
    //     {
    //         _didDogPass = true;
    //         // print("dog passed");
    //         
    //         if (_playerWaiting) StartsRunning();
    //     }
    //     else if (other.CompareTag("Player"))
    //     {
    //         if (_isRunning)
    //         {
    //             if (!_didDogPass)
    //             {
    //                 _playerWaiting = true;
    //                 _playerMove.SetCanMove(false);
    //             }
    //             else
    //             {
    //                 StartsRunning();
    //             }
    //         }
    //         else
    //         {
    //             _playerMove.StopAutoRun();
    //             CameraController.instance.ZoomIn();
    //         }
    //     }
    // }
}
