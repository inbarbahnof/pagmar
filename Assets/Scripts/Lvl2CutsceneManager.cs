using Audio.FMOD;
using UnityEngine;
using UnityEngine.Playables;

public class Lvl2CutsceneManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector sequence;
    [SerializeField] private PlayerMove _playerMove;
    private PlayerStateManager _playerStateManager;

    private bool _dogInPlace;
    private bool _playerInPlace;

    public void UpdateDogInPlace()
    {
        _dogInPlace = true;
        StartCutscene();
    }

    public void UpdatePlayerInPlace()
    {
        _playerInPlace = true;
        _playerStateManager ??= _playerMove.GetComponent<PlayerStateManager>();
        _playerStateManager.SetIdleState();
        _playerStateManager.UpdateStealth(true);
        _playerMove.SetCanMove(false);
        StartCutscene();
    }

    private void StartCutscene()
    {
        if (_playerInPlace && _dogInPlace && (sequence.state != PlayState.Playing))
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Chapter2DogEscapeMusic);

            sequence.Play();

        }
        
    }
}