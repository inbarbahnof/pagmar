using System;
using System.Collections;
using Interactables;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerMove _player;
    private PlayerStateManager _stateManager;
    private PlayerInput _input;
    private AimControl _aimControl;
    
    private Vector2 _lastAimInput = Vector2.zero;
    private Coroutine _retainAimCoroutine;
    
    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _player = GetComponent<PlayerMove>();
        _stateManager = GetComponent<PlayerStateManager>();
        _aimControl = GetComponent<AimControl>();
    }

    public void ChangeCallState(int state)
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
        
        // Debug.Log("ChangeCallState Call enabled: " + _input.actions["Call"].enabled);
        // Debug.Log("ChangeCallState CallMultiTap enabled: " + _input.actions["CallMultiTap"].enabled);
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputVal = context.ReadValue<Vector2>();
        
        if (_stateManager.CurrentState == PlayerState.Aim)
        {
            _player.UpdateMoveInput(Vector2.zero);
            
            if (inputVal != Vector2.zero)
            {
                _lastAimInput = inputVal;
                _player.UpdateAimInput(inputVal);
                _aimControl.UpdateAimInput(inputVal);

                if (_retainAimCoroutine != null)
                {
                    StopCoroutine(_retainAimCoroutine);
                    _retainAimCoroutine = null;
                }
            }
            else
            {
                if (_retainAimCoroutine == null)
                    _retainAimCoroutine = StartCoroutine(RetainLastAimInput());
            }
        }
        else if (_stateManager.IsClimbing)
        {
            _player.UpdateMoveInput(Vector2.zero);
        }
        else
        {
            _player.UpdateMoveInput(inputVal);
        }
    }
    
    private IEnumerator RetainLastAimInput()
    {
        _player.UpdateAimInput(_lastAimInput);
        _aimControl.UpdateAimInput(_lastAimInput);

        yield return new WaitForSeconds(0.5f);

        _player.UpdateAimInput(Vector2.zero);
        _aimControl.UpdateAimInput(Vector2.zero);

        _retainAimCoroutine = null;
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

    public void OnPet(InputAction.CallbackContext context)
    {
        _stateManager.UpdatePetting();
    }

    public void OnCall(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _stateManager.StartedCalling();
        }
    }
}
