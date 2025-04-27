using CheckpointUtils;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static CheckpointManager checkpointManagerInstance;
    
    void Start()
    {
        if (checkpointManagerInstance == null)
        {
            checkpointManagerInstance = new CheckpointManager(GetComponent<CheckpointOriginator>());
        }
        else Debug.LogError("TOO MANY CHECKPOINT MANAGERS!");

    }
    
}
