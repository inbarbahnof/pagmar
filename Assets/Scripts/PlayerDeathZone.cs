
using UnityEngine;

public class PlayerDeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.checkpointManagerInstance.Undo();
            print("player died");
        }
    }

    public void TrunOff()
    {
        gameObject.SetActive(false);
    }

}