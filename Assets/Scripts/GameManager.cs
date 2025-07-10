using System.Collections;
using Audio.FMOD;
using CheckpointUtils;
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

    private bool _inTutorialCutScene = false;
    
    public int ConnectionState => connectionState;
    public int Chapter => chapter;
    public bool InTutorialCutScene => _inTutorialCutScene;
    
    void Start()
    {
        // if (checkpointManagerInstance == null)
        // {
        //     checkpointManagerInstance = new CheckpointManager(GetComponent<CheckpointOriginator>());
        // }
        // else Debug.LogError("TOO MANY CHECKPOINT MANAGERS!");
        
        checkpointManagerInstance = new CheckpointManager(GetComponent<CheckpointOriginator>());

        if (instance == null)
        {
            instance = this;
        }
        else Debug.LogError("TOO MANY GAME MANAGERS!");
       
        PlayMusicAccordingToLevel();
        if (_cameraFade && _cameraFade.gameObject.activeInHierarchy) _cameraFade.FadeOutOverTime(true);
        StartCoroutine(WaitToZoom());
    }

    private IEnumerator WaitToZoom()
    {
        yield return new WaitForSeconds(0.5f);
        CameraController.instance.ZoomIn();
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
