using System.Collections;
using Audio.FMOD;
using CheckpointUtils;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static CheckpointManager checkpointManagerInstance;

    public static GameManager instance;

    [SerializeField] private int chapter;
    [SerializeField] private int connectionState;
    [SerializeField] private InputManager _playerInput;
    public int ConnectionState => connectionState;
    
    void Start()
    {
        if (checkpointManagerInstance == null)
        {
            checkpointManagerInstance = new CheckpointManager(GetComponent<CheckpointOriginator>());
        }
        else Debug.LogError("TOO MANY CHECKPOINT MANAGERS!");

        if (instance == null)
        {
            instance = this;
        }
        else Debug.LogError("TOO MANY GAME MANAGERS!");
       
        PlayMusicAccordingToLevel();
    }

    private void PlayMusicAccordingToLevel()
    {
        switch (chapter)
        {
            case 0:
                AudioManager.Instance.PlayMusic(FMODEvents.Instance.Chapter0Music);
                break;
            case 1:
                AudioManager.Instance.PlayMusic(FMODEvents.Instance.Chapter1Music);
                break;
            case 2:
                AudioManager.Instance.PlayMusic(FMODEvents.Instance.Chapter2Music);
                break;
        }
        
        AudioManager.Instance.PlayAmbiance(FMODEvents.Instance.Ambiance);
    }

    private IEnumerator ChangeBinding()
    {
        yield return new WaitForSeconds(0.1f);
        _playerInput.ChangeCallState(connectionState);
    }
    
}
