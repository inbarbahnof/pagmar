using CheckpointUtils;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static CheckpointManager checkpointManagerInstance;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (checkpointManagerInstance == null)
        {
            checkpointManagerInstance = new CheckpointManager(GetComponent<CheckpointOriginator>());
        }
        else Debug.LogError("TOO MANY CHECKPOINT MANAGERS!");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
