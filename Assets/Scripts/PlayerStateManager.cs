using System;
using System.Collections;
using Audio.FMOD;
using DG.Tweening;
using Interactables;
using UnityEngine;
using UnityEngine.AI;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] private float _pickUpAnimTime = 0.5f;
    [SerializeField] private float _throwAnimTime = 0.867f;
    [SerializeField] private float _waitOnCallTime = 1f;
    [SerializeField] private float _climbAnimTime = 2f;
    
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
    private bool _isPushingFromLeft;
    private bool _isClimbing;

    private Coroutine _waitToCallCoroutine;
    
    private PlayerAnimationComputer _computer;

    public PlayerState CurrentState => curState;
    public bool IsPushingFromLeft => _isPushingFromLeft;

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
        if (_curInteraction is PushInteractable pushInteractable)
            _isPushingFromLeft = pushInteractable.IsPushingFromLeft;
        else _isPushingFromLeft = false;
        
        PlayerAnimationInput input = new PlayerAnimationInput(curState, _isCrouching, 
            _move.IsMoving, _isCalling, _move.MovingRight, _pickedUp,
            _justPickedUp, _throwing, _isPushingFromLeft);
        
        PlayerAnimation animation = _computer.Compute(input);
        _animationManager.PlayerAnimationUpdate(animation);
        
        // print("player state " + curState);
    }

    public void UpdatePlayerSpeed(bool isFast)
    {
        _animationManager.PlayerRunSpeed(isFast);
    }

    public void UpdateAimAbility(bool canAim = false)
    {
        _isAbleToAim = canAim;
    }

    public void UpdateClimbing()
    {
        SetState(PlayerState.Climb);
        _isClimbing = true;
        
        StartCoroutine(WaitForClimbAnim());
    }

    private IEnumerator WaitForClimbAnim()
    {
        yield return new WaitForSeconds(_climbAnimTime);

        _isClimbing = false;
        ClimbInteractableManager.instance.StopInteraction();
    }

    public void UpdatePickedUp(bool pick)
    {
        _pickedUp = pick;
        
        _justPickedUp = true;
        StartCoroutine(WaitForPickUpAnim());
        
        // if (_pickedUp)
        // {
        //     _justPickedUp = true;
        //     StartCoroutine(WaitForPickUpAnim());
        // }
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

    public void OnFinishedInteraction(BaseInteractable interactable = null)
    {
        SetState(PlayerState.Idle);
        //UpdatePickedUp(false);
    }

    public void UpdatePetting()
    {
        SetState(PlayerState.Pet);
    }

    public void StartedCalling()
    {
        if (_isCalling) return;
        
        _isCalling = true;
        
        _move.SetCanMove(false);
        _move.UpdateMoveInput(Vector2.zero);
        
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.PlayerCall);
        
        if (_waitToCallCoroutine != null) StopCoroutine(_waitToCallCoroutine);
            
        _waitToCallCoroutine = StartCoroutine(WaitToCall());
    }
    
    private IEnumerator WaitToCall()
    {
        yield return new WaitForSeconds(_waitOnCallTime);
        
        UpdateCalling();

        yield return new WaitForSeconds(0.1f);
        
        _isCalling = false;
        _waitToCallCoroutine = null;
    }

    public void UpdateCalling()
    {
        SetState(PlayerState.Call);
        _move.SetCanMove(true);
    }

    public void UpdateWalking(bool isWalking)
    {
        if (_isCalling) return;
        
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
                if (!_throwing)
                {
                    UpdatePickedUp(false);
                    AudioManager.Instance.PlayOneShot(FMODEvents.Instance.PlayerPickUp);
                }
                break;
        }
    }

    private void SetState(PlayerState newState)
    {
        if (curState == newState || _isClimbing) return;
        
        if (_isAbleToAim) curState = PlayerState.Aim;
        else curState = newState;
    }

    private void SetStateAccordingToInteraction(IInteractable interactable)
    {
        if (interactable is PushInteractable push)
        {
            _isPushingFromLeft = push.IsPushingFromLeft;
            _move.CheckIfNeedToFlipArt();
            SetState(PlayerState.Push);
        }
    }
}
