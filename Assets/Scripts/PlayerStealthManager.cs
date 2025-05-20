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
        _playerStateManager.UpdateStealth(_isProtected);

    }
}
