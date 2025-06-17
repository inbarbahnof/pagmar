using Audio.FMOD;
using UnityEngine;

public class PlayerStealthManager : MonoBehaviour
{
    private PlayerStateManager _playerStateManager;
    private bool _isProtected;

    public bool isProtected => _isProtected; 

    private void Start()
    {
        _playerStateManager = GetComponent<PlayerStateManager>();
    }

    public void SetStealthMode(bool isInStealth)
    {
        _isProtected = isInStealth;
    }

    public void StealthObstacle(bool isInStealth)
    {
        _playerStateManager.UpdateStealth(isInStealth);
        
        if (isInStealth) 
            AudioManager.Instance.SetFloatParameter(default,
                "Stealth Mode", 1, true);
        else
            AudioManager.Instance.SetFloatParameter(default,
                "Stealth Mode", 0, true);
    }

    public void SetProtected(bool isInStealth)
    {
        _isProtected = isInStealth;
    }
}
