using System;
using Audio.FMOD;
using FMOD.Studio;
using UnityEngine;

public class CutSceneManagerLvl4 : MonoBehaviour
{
    [SerializeField] private Transform _playerPos;
    [SerializeField] private Transform _dogPos;
    [SerializeField] private SmoothMover _player;
    [SerializeField] private SmoothMover _dog;
    
    private StartCutsceneManager _cutsceneManager;
    private bool _waiting;

    private EventInstance _runningMusic;

    private void Start()
    {
        _cutsceneManager = GetComponent<StartCutsceneManager>();
    }
    
    public void MoveToPositions()
    {
        _cutsceneManager.FreezePLayer();
        _player.MoveTo(_playerPos.position, 4f);
        _dog.MoveTo(_dogPos.position, 4f);
        _waiting = true;
        
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.MuteAmbienceEvent();
    }

    private void Update()
    {
        if (!_waiting) return;

        if (Vector3.Distance(_player.transform.position, _playerPos.position) < 0.3
            && Vector3.Distance(_dog.transform.position, _dogPos.position) < 0.8)
        {
            _cutsceneManager.ShowSequence();
            _waiting = false;
            
            _runningMusic = AudioManager.Instance.PlayLoopingSound(FMODEvents.Instance.ChapterRunning4Music);
            AudioManager.Instance.SetFloatParameter(
                _runningMusic,
                "Ending Run",
                1,
                false);
        }
    }
}
