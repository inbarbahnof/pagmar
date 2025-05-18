using System;
using Interactables;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    private PlayerState curState = PlayerState.Idle;
    private PlayerAnimationManager _animationManager;
    private PlayerStealthManager _stealthManager;
    private bool _isCrouching;

    public PlayerState CurrentState => curState;

    // public delegate void OnStateChange(PlayerState newState);

    private void Start()
    {
        GetManagers();
    }

    private void GetManagers()
    {
        _animationManager = GetComponent<PlayerAnimationManager>();
        _stealthManager = GetComponent<PlayerStealthManager>();
    }

    private void Update()
    {
        _isCrouching = _stealthManager.isProtected;
        PlayerAnimationInput input = new PlayerAnimationInput(curState, _isCrouching);
        
        _animationManager.PlayerAnimationUpdate(input);
    }

    public void SetState(PlayerState newState)
    {
        if (curState != newState)
        {
            curState = newState;
        }
    }

    public void SetStateAccordingToInteraction(IInteractable interactible)
    {
        if (interactible is PushInteractable push)
        {
            SetState(PlayerState.Push);
        }
    }
}
