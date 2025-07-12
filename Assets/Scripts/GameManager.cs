using System.Collections;
using Audio.FMOD;
using CheckpointUtils;
using Dog;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    [Header("Die Effects")]
    [SerializeField] private GameObject _ghostieEffectgameObject;
    [SerializeField] private GameObject _ghostEffectgameObject;

    private bool _inTutorialCutScene = false;
    private PlayerMove _playerMove;
    private IObjectFader _fader;
    private DieEffect _ghostieEffect;
    private DieEffect _ghostEffect;
    
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
        
        PlayMusicAccordingToLevel();
        if (_cameraFade && _cameraFade.gameObject.activeInHierarchy) _cameraFade.FadeOutOverTime(true);
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
                break;
            case 4:
                AudioManager.Instance.PlayMusic(FMODEvents.Instance.Chapter4Music);
                break;
        }
        
        AudioManager.Instance.PlayAmbiance(FMODEvents.Instance.Ambiance);
    }

    public void StartGame()
    {
        if (_startScreen != null) _startScreen.OnPressStart();
    }

    public void PlayerDied(bool isGhostie)
    {
        _playerMove.SetCanMove(false);
        
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

    public void TurnToPhase2()
    {
        connectionState = 2;
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
            Application.Quit();
        }
    }
    
}
