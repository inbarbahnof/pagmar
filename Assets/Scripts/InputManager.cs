using System;
using Interactables;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerMove _player;
    private PlayerStateManager _stateManager;
    private PlayerInput _input;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        
        _player = GetComponent<PlayerMove>();
        _stateManager = GetComponent<PlayerStateManager>();
    }

    public void ChangeState(int state)
    {
        if (state <= 2)
        {
            _input.actions["Call"].Disable();
            _input.actions["CallMultiTap"].Enable();
        }
        else
        {
            _input.actions["CallMultiTap"].Disable();
            _input.actions["Call"].Enable();
        }
        
        // Debug.Log("ChangeState Call enabled: " + _input.actions["Call"].enabled);
        // Debug.Log("ChangeState CallMultiTap enabled: " + _input.actions["CallMultiTap"].enabled);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        _player.UpdateMoveInput(moveInput);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            InteractableManager.instance.OnInteract();  // Button just pressed
        }
        else if (context.canceled)
        {
            InteractableManager.instance.OnStopInteract(); // Button released
        }
    }

    public void OnCall(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _stateManager.SetState(PlayerState.Call);
            print("call performed");
        }
    }
}
