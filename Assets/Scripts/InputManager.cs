using System;
using System.Collections;
using Audio.FMOD;
using Interactables;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerMove _player;
    private PlayerStateManager _stateManager;
    private PlayerInput _input;
    private AimControl _aimControl;
    private bool _canMoveInput = true;
    private bool _hasGameStarted;
    
    private Vector2 _lastAimInput = Vector2.zero;
    private Coroutine _retainAimCoroutine;

    private TextAppear callListener;

    [SerializeField] private float deadzoneThreshold = 0.2f;
    
    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _player = GetComponent<PlayerMove>();
        _stateManager = GetComponent<PlayerStateManager>();
        _aimControl = GetComponent<AimControl>();
    }

    private void Start()
    {
        StartCoroutine(ResetInput());
    }

    private IEnumerator ResetInput()
    {
        yield return new WaitForSeconds(2f);
        _input.enabled = false;
        _input.enabled = true;
        _input.SwitchCurrentActionMap("Player");
    }

    public void SwitchActionMaps(bool uiInput)
    {
        if (uiInput) _input.SwitchCurrentActionMap("UI");
        else _input.SwitchCurrentActionMap("Player");
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputVal = context.ReadValue<Vector2>();
        if (inputVal.magnitude < deadzoneThreshold)
            inputVal = Vector2.zero;
        
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
        else if (_stateManager.IsClimbing || _stateManager.IsDropping)
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
        _player.UpdateMoveInput(Vector2.zero);
        _player.UpdateAimInput(_lastAimInput);
        _aimControl.UpdateAimInput(_lastAimInput);

        yield return new WaitForSeconds(0.5f);

        _player.UpdateAimInput(Vector2.zero);
        _aimControl.UpdateAimInput(Vector2.zero);

        _retainAimCoroutine = null;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(GameManager.instance.Chapter == 0 && !_hasGameStarted)
        {
            GameManager.instance.StartGame();
            _hasGameStarted = true;
            return;
        }
        
        if (!_canMoveInput) return;
        if (context.started)
        {
            InteractableManager.instance.OnInteract();  // Button just pressed
        }
        else if (context.canceled)
        {
            InteractableManager.instance.OnStopInteract(); // Button released
        }
    }

    public void CanPetOnly()
    {
        _canMoveInput = false;
    }

    public void OnPet(InputAction.CallbackContext context)
    {
        _stateManager.UpdatePetting();
        
        if (!_canMoveInput) GameManager.instance.OnPlayerPetLevel3();
        _canMoveInput = true;
    }

    public void RegisterCallListener(TextAppear listener)
    {
        callListener = listener;
    }

    public void OnCall(InputAction.CallbackContext context)
    {
        if (!_canMoveInput || GameManager.instance.Chapter == 5) return;
        if (context.started)
        {
            if (callListener)
            {
                callListener.StopShowText();
                callListener = null;
            }
            _stateManager.StartedCalling();
        }
    }

    public void OnMenuButton(InputAction.CallbackContext context)
    {
        if (context.performed) GameManager.instance.OnMenuButton();
    }
}
