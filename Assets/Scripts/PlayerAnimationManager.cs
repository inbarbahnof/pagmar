using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private Animator _animator;
    private PlayerState _curState;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayerAnimationUpdate(PlayerAnimationInput input)
    {
        if (_curState != input._playerState)
        {
            switch (input._playerState)
            {
                case PlayerState.Stealth:
                    _animator.SetBool("Crouching", true);
                    break;
                case PlayerState.Idle:
                    if (!input._isCrouching)
                        _animator.SetBool("Crouching", false);
                    break;
            }
        
            _curState = input._playerState;
        }
    }
}
