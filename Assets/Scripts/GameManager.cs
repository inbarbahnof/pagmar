using System.Collections;
using CheckpointUtils;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static CheckpointManager checkpointManagerInstance;

    public static GameManager instance;

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

        StartCoroutine(ChangeBinding());
    }

    private IEnumerator ChangeBinding()
    {
        yield return new WaitForSeconds(0.1f);
        _playerInput.ChangeCallState(connectionState);
    }
    
}
