using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    
    [SerializeField] private CinemachineCamera _followPlayer;
    [SerializeField] private CinemachineCamera _followPlayerAndDog;
    
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Debug.LogError("TOO MANY CAMERA CONTROLLERS!");

        FollowPlayer();
    }

    public void FollowPlayer()
    {
        _followPlayer.enabled = true;
        _followPlayerAndDog.enabled = false;
    }

    public void FollowPlayerAndDog()
    {
        _followPlayerAndDog.enabled = true;
        _followPlayer.enabled = false;
    }
}
