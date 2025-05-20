using System;
using Interactables;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public enum ThrowState
    {
        Aim,
        Throw,
        End
    };
    
    private PlayerState curState = PlayerState.Idle;
    private PlayerAnimationManager _animationManager;
    private PlayerStealthManager _stealthManager;
    private bool _isCrouching;
    private bool _isAbleToAim;

    public PlayerState CurrentState => curState;
    private BaseInteractable _curInteraction;

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
        PlayerAnimationInput input = new PlayerAnimationInput(curState, _isCrouching);
        
        _animationManager.PlayerAnimationUpdate(input);
    }

    public void UpdateAimAbility(bool canAim = false)
    {
        _isAbleToAim = canAim;
    }

    public void UpdateCurInteraction(BaseInteractable interactable)
    {
        _curInteraction = interactable;
        SetStateAccordingToInteraction(interactable);
    }

    public void OnFinishedInteraction(BaseInteractable interactable)
    {
        SetState(PlayerState.Idle);
    }

    public void UpdatePetting()
    {
        SetState(PlayerState.Pet);
    }

    public void UpdateCalling()
    {
        SetState(PlayerState.Call);
    }

    public void UpdateWalking(bool isWalking)
    {
        SetState(isWalking ? PlayerState.Walk : PlayerState.Idle);
    }

    public void UpdateStealth(bool isProtected)
    {
        _isCrouching = isProtected;
        if (isProtected) SetState(PlayerState.Stealth);
    }

    public void UpdateThrowState(ThrowState state)
    {
        switch (state)
        {
            case (ThrowState.Aim):
                SetState(PlayerState.Aim);
                break;
            case (ThrowState.Throw):
                SetState(PlayerState.Throw);
                break;
            case (ThrowState.End):
                SetState(PlayerState.Idle);
                break;
        }
    }

    private void SetState(PlayerState newState)
    {
        if (curState == newState) return;
        if (_isAbleToAim) curState = PlayerState.Aim;
        else curState = newState;
    }

    private void SetStateAccordingToInteraction(IInteractable interactible)
    {
        if (interactible is PushInteractable push)
        {
            SetState(PlayerState.Push);
        }
    }
}
