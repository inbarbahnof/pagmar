
using UnityEngine;

public class PlayerDeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // reset
            print("player died");
        }
    }

    public void TrunOff()
    {
        this.gameObject.SetActive(false);
    }

}