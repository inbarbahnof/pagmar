using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1.4f, - 10);
    }
}
