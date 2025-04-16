using System;
using Interactabels;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    private PlayerState curState = PlayerState.Idle;

    public PlayerState CurrentState => curState;

    public delegate void OnStateChange(PlayerState newState);

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
