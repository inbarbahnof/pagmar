using UnityEngine;

public class SceneEndPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) GameManager.instance.LevelEnd();
    }
}