using System;
using System.Collections;
using Audio.FMOD;
using DG.Tweening;
using FMOD.Studio;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Playables;
using WaitForEndOfFrame = UnityEngine.WaitForEndOfFrame;

public class DogMeetManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector dogSequence;
    [SerializeField] private PlayableDirector ghostieSequence;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private GameObject playerBlockCollider;
    [SerializeField] private SpriteFade bushGlow;
    private bool _playerHiding;
    private bool _playerHidePrompt;

    private EventInstance _cutsceneMusic;

    private PlayerStateManager _playerStateManager;
    
    public void ShowDogSequence()
    {
        // activate upon player reach start pos
        // freeze player
        // play sequence to pan camera right and move dog
        dogSequence.Play();
        _playerHidePrompt = true;
        _cutsceneMusic = AudioManager.Instance.PlayLoopingSound(FMODEvents.Instance.Chapter0Cutscene);

        // on camera pan stop allow player controls and show 'hide' prompt
    }

    public void PlayerHiding(bool hiding)
    {
        _playerHiding = hiding;
        UpdateBushGlow(!hiding);
    }
    public void UpdateBushGlow(bool glow)
    {
        if (_playerHidePrompt)
        {
            bushGlow.FadeOutOverTime(glow);
        }
    }
    
    public void ShowGhostiesSequence()
    {
        // activate upon player reached bush
        // freeze player
        // play sequence to pam cam left and activate ghosties them move dog and scatter
        StartCoroutine(WaitToShowGhostiesCoroutine());
    }

    private void pausePlayerInBush()
    {
        playerMove.SetCanMove(false);
        _playerStateManager ??= playerMove.GetComponent<PlayerStateManager>();
        _playerStateManager.StopIdle();
        _playerStateManager.UpdateStealth(true);
    }

    private IEnumerator WaitToShowGhostiesCoroutine()
    {
        float startTime = Time.time;
        float timeout = 30f;
        while (!_playerHiding && Time.time - startTime < timeout)
        {
            yield return null; // wait one frame
        }
        
        if (_cutsceneMusic.isValid())
            AudioManager.Instance.SetFloatParameter(_cutsceneMusic, "Lvl0 Cutscene", 1, false);
        
        pausePlayerInBush();
        playerBlockCollider.SetActive(false);
        ghostieSequence.Play();
        _playerHidePrompt = false;
    }
}
