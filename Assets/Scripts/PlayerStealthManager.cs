using Audio.FMOD;
using UnityEngine;

public class PlayerStealthManager : MonoBehaviour
{
    private PlayerStateManager _playerStateManager;
    private PlayerMove _playerMove;
    private bool _isProtected;

    public bool isProtected => _isProtected; 

    private void Start()
    {
        _playerStateManager = GetComponent<PlayerStateManager>();
        _playerMove = GetComponent<PlayerMove>();
    }

    public void SetStealthMode(bool isInStealth)
    {
        _isProtected = isInStealth;
        
        if (GameManager.instance.Chapter == 0)
            AudioManager.Instance.SetFloatParameter(default,
              "Stealth Mode", 0, true);
        else
        {
            if (isInStealth)
                AudioManager.Instance.SetFloatParameter(default,
                    "Stealth Mode", 1, true);
            else
                AudioManager.Instance.SetFloatParameter(default,
                    "Stealth Mode", 0, true);
        }
      
    }

    public void StealthObstacle(bool isInStealth)
    {
        _playerStateManager.UpdateStealth(isInStealth);
        _playerMove.UpdateCrouch(isInStealth);
    }

    public void SetProtected(bool isInStealth)
    {
        _isProtected = isInStealth;
    }
}
