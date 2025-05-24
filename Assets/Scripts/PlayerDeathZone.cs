
using UnityEngine;

public class PlayerDeathZone : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.checkpointManagerInstance.Undo();
            print("player died");
        }
    }

    protected void DogDied()
    {
        print("dog died");
        GameManager.checkpointManagerInstance.Undo();
    }
}