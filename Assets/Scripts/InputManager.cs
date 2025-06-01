using System;
using System.Collections;
using Interactables;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private float _waitOnCallTime = 0.5f;
    
    private PlayerMove _player;
    private PlayerStateManager _stateManager;
    private PlayerInput _input;
    private AimControl _aimControl;

    private bool _canWalk = true;
    private bool _called;
    private Coroutine _waitToCallCoroutine;
    
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
        if (!_canWalk)  return;
        if (_called)
        {
            _stateManager.UpdateCalling();
            _called = false;
        }
        
        Vector2 inputVal = context.ReadValue<Vector2>();
        
        if (_stateManager.CurrentState == PlayerState.Aim)
        {
            _player.UpdateMoveInput(Vector2.zero);
            _aimControl.UpdateAimInput(inputVal);
        }
        else
        {
            _player.UpdateMoveInput(inputVal);
        }
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
            _canWalk = false;
            _called = true;
            _player.UpdateMoveInput(Vector2.zero);
        }
        
        if (context.performed)
        {
            if (_waitToCallCoroutine != null) StopCoroutine(_waitToCallCoroutine);
            
            _waitToCallCoroutine = StartCoroutine(WaitToCall());
        }
    }

    private IEnumerator WaitToCall()
    {
        yield return new WaitForSeconds(_waitOnCallTime);
        
        _stateManager.UpdateCalling();
        
        _canWalk = true;
        _waitToCallCoroutine = null;
    }
    
}
