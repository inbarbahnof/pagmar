using System;
using System.Collections;
using Audio.FMOD;
using Interactables;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    private PlayerMove _player;
    private PlayerStateManager _stateManager;
    private PlayerInput _input;
    private AimControl _aimControl;
    private bool _canMoveInput = true;
    private bool _canPet = true;
    private bool _hasGameStarted;
    private bool _canCall = true;
    
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
        if (!_canMoveInput) return;
        
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
        if (!_canMoveInput || GameManager.instance.Chapter == 5) return;
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
        _canPet = true;
    }

    public void PlayerDisableAllInput()
    {
        _canMoveInput = false;
        _canPet = false;
    }
    
    public void PlayerEnableAllInput()
    {
        _canMoveInput = true;
        _canPet = true;
    }

    public void OnPet(InputAction.CallbackContext context)
    {
        if (GameManager.instance.Chapter == 5 || !_canPet) return;
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
        if (!_canMoveInput || GameManager.instance.Chapter == 5 || !_canCall) return;
        if (context.started)
        {
            ResetCallPrompt();
            _stateManager.StartedCalling();
        }
    }

    public void ResetCallPrompt()
    {
        if (callListener)
        {
            callListener.StopShowText();
            callListener = null;
        }
    }

    public void OnMenuButton(InputAction.CallbackContext context)
    {
        if (context.performed) GameManager.instance.OnMenuButton();
    }

    public void DisableCall()
    {
        _canCall = false;
    }
    
    public void CrisisMode()
    {
        Time.timeScale = 1;
        
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.StopAllLoopShots();
        AudioManager.Instance.StopAllSnapshots();
        
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
