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
        CameraController.instance.ZoomOut();
        print("player stop running");
        
        AudioManager.Instance.SetFloatParameter(
            AudioManager.Instance.musicInstance,
            "Ending Run",
            2,
            false);
    }

    private IEnumerator StartRunCorutine()
    {
        DogRun();
        yield return new WaitForSeconds(1f);
        _playerMove.StartAutoRunWithVerticalControl();

        AudioManager.Instance.SetFloatParameter(
            AudioManager.Instance.musicInstance,
            "Ending Run",
            1,
            false);
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
}
