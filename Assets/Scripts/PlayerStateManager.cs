using System;
using System.Collections;
using Interactables;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] private float _pickUpAnimTime = 0.5f;
    [SerializeField] private float _throwAnimTime = 0.867f;
    
    public enum ThrowState
    {
        Aim,
        Throw,
        End
    };
    
    private PlayerState curState = PlayerState.Idle;
    private PlayerAnimationManager _animationManager;
    private PlayerStealthManager _stealthManager;
    private BaseInteractable _curInteraction;
    private PlayerMove _move;
    private InputManager _inputManager;
    
    private bool _isCrouching;
    private bool _isAbleToAim;
    private bool _isCalling;
    private bool _pickedUp;
    private bool _justPickedUp;
    private bool _throwing;

    private PlayerAnimationComputer _computer;

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
        _move = GetComponent<PlayerMove>();
        _computer = new PlayerAnimationComputer();
        _inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        PlayerAnimationInput input = new PlayerAnimationInput(curState, _isCrouching, 
            _move.IsMoving, _isCalling, _move.MovingRight, _pickedUp,
            _justPickedUp, _throwing);
        
        PlayerAnimation animation = _computer.Compute(input);
        _animationManager.PlayerAnimationUpdate(animation);
        
        // print("player state " + curState);
    }

    public void UpdateAimAbility(bool canAim = false)
    {
        _isAbleToAim = canAim;
    }

    public void UpdatePickedUp(bool pick)
    {
        _pickedUp = pick;
        if (_pickedUp)
        {
            _justPickedUp = true;
            StartCoroutine(WaitForPickUpAnim());
        }
    }
    
    private IEnumerator WaitForThrowingAnim()
    {
        yield return new WaitForSeconds(_throwAnimTime);
        _throwing = false;
    }

    private IEnumerator WaitForPickUpAnim()
    {
        yield return new WaitForSeconds(_pickUpAnimTime);
        _justPickedUp = false;
    }

    public void UpdateCurInteraction(BaseInteractable interactable)
    {
        _curInteraction = interactable;
        SetStateAccordingToInteraction(_curInteraction);
    }

    public void OnFinishedInteraction(BaseInteractable interactable)
    {
        SetState(PlayerState.Idle);
    }

    public void UpdatePetting()
    {
        SetState(PlayerState.Pet);
    }

    public void StartedCalling()
    {
        _isCalling = true;
    }

    public void UpdateCalling()
    {
        SetState(PlayerState.Call);
    }

    public void UpdateWalking(bool isWalking)
    {
        if (!_isCrouching) SetState(isWalking ? PlayerState.Walk : PlayerState.Idle);
        else SetState(PlayerState.Stealth);
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
                _throwing = true;
                StartCoroutine(WaitForThrowingAnim());
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

        if (newState == PlayerState.Call)
        {
            StartCoroutine(WaitToStopCall());
        }
        
    }

    private IEnumerator WaitToStopCall()
    {
        yield return new WaitForSeconds(_inputManager.WaitOnCallTime);
        _isCalling = false;
    }

    private void SetStateAccordingToInteraction(IInteractable interactable)
    {
        if (interactable is PushInteractable push)
        {
            SetState(PlayerState.Push);
        }
    }
}
