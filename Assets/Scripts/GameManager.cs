using System.Collections;
using Audio.FMOD;
using CheckpointUtils;
using Dog;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static CheckpointManager checkpointManagerInstance;
    public static GameManager instance;

    [SerializeField] private int chapter;
    [SerializeField] private int connectionState;
    [SerializeField] private InputManager _playerInput;
    [SerializeField] private CameraFade _cameraFade;
    [SerializeField] private DogActionManager _dog;
    [SerializeField] private GameObject _faderGameObject;
    [SerializeField] private StartScreenManager _startScreen;
    [SerializeField] private GameObject _blackScreen;
    
    [Header("Die Effects")]
    [SerializeField] private GameObject _ghostieEffectgameObject;
    [SerializeField] private GameObject _ghostEffectgameObject;
    [SerializeField] private GameObject[] _allLevelGhostst;

    [SerializeField] private MenuManager menuManager;

    private bool _inTutorialCutScene = false;
    private PlayerMove _playerMove;
    private PlayerStealthManager _playerStealth;
    private PlayerStateManager _playerState;
    private IObjectFader _fader;
    private DieEffect _ghostieEffect;
    private DieEffect _ghostEffect;

    private Menus _curMenu = Menus.NONE;
    
    public int ConnectionState => connectionState;
    public int Chapter => chapter;
    public bool InTutorialCutScene => _inTutorialCutScene;
    
    void Start()
    {
        checkpointManagerInstance = new CheckpointManager(GetComponent<CheckpointOriginator>());

        if (instance == null) instance = this;
        else Debug.LogError("TOO MANY GAME MANAGERS!");
        
        _cameraFade.gameObject.SetActive(true);
        
        _fader = _faderGameObject.GetComponent<IObjectFader>();
        _ghostieEffect = _ghostieEffectgameObject.GetComponent<DieEffect>();
        _ghostEffect = _ghostEffectgameObject.GetComponent<DieEffect>();
        _playerMove = _playerInput.GetComponent<PlayerMove>();
        _playerStealth = _playerInput.GetComponent<PlayerStealthManager>();
        _playerState = _playerInput.GetComponent<PlayerStateManager>();
        
        if (chapter != 0)
        {
            PlayMusicAccordingToLevel();
            
            if (_cameraFade && _cameraFade.gameObject.activeInHierarchy)
                _cameraFade.FadeOutOverTime(true);
        }
        
        PlayAmbiance();
        StartCoroutine(WaitToZoom());
    }

    private IEnumerator WaitToZoom()
    {
        yield return new WaitForSeconds(0.5f);
        CameraController.instance.ZoomIn(0.8f);
    }

    private void PlayMusicAccordingToLevel()
    {
        switch (chapter)
        {
            case 0:
                AudioManager.Instance.PlayMusic(FMODEvents.Instance.Chapter0Music);
                _inTutorialCutScene = true;
                break;
            case 1:
                AudioManager.Instance.PlayMusic(FMODEvents.Instance.Chapter1Music);
                break;
            case 2:
                AudioManager.Instance.PlayMusic(FMODEvents.Instance.Chapter2Music);
                break;
            case 3:
                AudioManager.Instance.PlayMusic(FMODEvents.Instance.Chapter3Music);
                _playerState.PlayerWorried(true);
                break;
            case 4:
                AudioManager.Instance.PlayMusic(FMODEvents.Instance.Chapter4Music);
                break;
        }
    }
    
    private void PlayAmbiance()
    {
        AudioManager.Instance.PlayAmbiance(FMODEvents.Instance.Ambiance);
    }

    public void StartGame()
    {
        if (_startScreen != null) _startScreen.OnPressStart();
        // AudioManager.Instance.MuteAmbienceEvent();
        PlayMusicAccordingToLevel();
    }

    public void PlayerDied(bool isGhostie)
    {
        _playerInput.ResetCallPrompt();
        _playerMove.SetCanMove(false);
        _playerStealth.SetProtected(true);
        _playerState.PlayerScared();
        // _dog.HandleDogProtectionChanged(true);

        foreach (var ghost in _allLevelGhostst)
        {
            ghost.SetActive(false);
        }
        
        if (isGhostie) _ghostieEffect.PlayEffect();
        else _ghostEffect.PlayEffect();

        StartCoroutine(PlayerDiedCoroutine());
    }

    private IEnumerator PlayerDiedCoroutine()
    {
        yield return new WaitForSeconds(0.8f);
        checkpointManagerInstance.Undo();

        yield return new WaitForSeconds(0.5f);
        _playerMove.SetCanMove(true);
        _playerStealth.SetProtected(false);
        
        yield return new WaitForSeconds(1.3f);
        foreach (var ghost in _allLevelGhostst)
        {
            ghost.SetActive(true);
        }
    }

    public void PlayVolumeEffect()
    {
        _fader.FadeOutOverTime(true);
    }
    
    public void StopVolumeEffect()
    {
        _fader.FadeOutOverTime();
    }

    public void OnPlayerPetLevel3()
    {
        _dog.StopWaitingForPet();
        
        AudioManager.Instance.SetFloatParameter(AudioManager.Instance.musicInstance,
            "Lvl3 Cutscene", 1, false);
    }

    public void PlayReunionMusic()
    {
        AudioManager.Instance.PlayMusic(FMODEvents.Instance.Chapter3ReunionMusic);
    }

    public void TutorialOut()
    {
        _inTutorialCutScene = false;
        // AudioManager.Instance.ResumeAmbience();
    }

    public void LevelEnd()
    {
        if (!_cameraFade)
        {
            ChangeScene();
            return;
        }
        if (!_cameraFade.gameObject.activeInHierarchy) _cameraFade.gameObject.SetActive(true);
        StartCoroutine(LevelEndCoroutine());
    }

    public void EndGame()
    {
        _blackScreen.SetActive(true);
        StartCoroutine(WaitToEnd());
    }

    private IEnumerator WaitToEnd()
    {
        yield return new WaitForSeconds(1f);
        ChangeScene();
    }

    public void TurnToPhase2()
    {
        connectionState = 2;
    }

    public void Chapter5()
    {
        chapter = 5;
    }

    public void Chapter4()
    {
        chapter = 4;
    }

    private IEnumerator LevelEndCoroutine()
    {
        Time.timeScale = 0;
        _cameraFade.FadeOutOverTime();
        AudioManager.Instance.StopMusic();
        yield return new WaitForSecondsRealtime(_cameraFade.Duration);
        ChangeScene();
    }
    
    private void ChangeScene()
    {
        Time.timeScale = 1;
        int nextSceneIdx = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings > nextSceneIdx)
            SceneManager.LoadScene(nextSceneIdx);
        else
        {
            print("end");
            StartCoroutine(GameEnd());
        }
    }

    private IEnumerator GameEnd()
    {
        yield return new WaitForSeconds(2f);
        ShowEndCredits();
    }

    public void OnMenuButton()
    {
        switch (_curMenu)
        {
            case Menus.NONE:
                menuManager.ShowPauseMenu(true);
                _curMenu = Menus.PAUSE;
                _playerInput.SwitchActionMaps(true);
                //Time.timeScale = 0;
                break;
            case Menus.PAUSE:
                //Time.timeScale = 1;
                menuManager.ShowPauseMenu(false);
                _curMenu = Menus.NONE;
                _playerInput.SwitchActionMaps(false);
                break;
            case Menus.CREDITS:
                break;
        }
        
    }

    private void ShowEndCredits()
    {
        menuManager.ShowCredits();
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
