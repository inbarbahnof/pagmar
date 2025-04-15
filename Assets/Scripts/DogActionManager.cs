using System;
using DefaultNamespace;
using UnityEngine;

public class DogActionManager : MonoBehaviour
{
    private DogState curState = DogState.Idle;
    public DogState CurState => curState;
    
    [SerializeField] private PlayerStateManager playerStateManager;
    [SerializeField] private RandomTargetGenerator _targetGenerator;
    [SerializeField] private float _pushDistance = 2f;
    
    private PlayerFollower _playerFollower;
    private Transform _playerTransform;

    private bool _dogReachedTarget;
    private bool _dogFollowingTarget;
    private bool _dogFollowingTOI;

    private DogStateComputer _computer;
    

    private void Start()
    {
        _computer = new DogStateComputer();
        
        _playerFollower = GetComponent<PlayerFollower>();
        
        _playerFollower.OnIdleAfterTarget += HandleDogIdle;
        _playerFollower.OnStartFollowTOI += HandleDogFollowTOI;
        _playerFollower.OnStartFollow += HandleDogFollow;

        _playerTransform = playerStateManager.GetComponent<Transform>();

        StartWalkingAfterNextTarget();
    }
    
    private void Update()
    {
        float curDistance = Vector2.Distance(_playerTransform.position, transform.position);
        DogStateMachineInput newInput = new DogStateMachineInput(playerStateManager.CurrentState, curDistance, 
            _dogReachedTarget, _dogFollowingTarget, _dogFollowingTOI, _pushDistance);
        DogState newState = _computer.Compute(curState, newInput);

        if (curState != newState)
        {
            HandleDogStateChange(newState);
        }
    }

    private void HandleDogStateChange(DogState newState)
    {
        
    }

    private void HandlePlayerPushBehavior()
    {
        float distance = Vector2.Distance(_playerTransform.position, transform.position);
        
        if (distance < _pushDistance)
        {
            curState = DogState.Push;
            _playerFollower.SetIsGoingToTarget(false);
        }
    }
    
    private void HandlePlayerWalkBehavior()
    {
        // float distanceToPlayer = Vector3.Distance(transform.position, playerStateManager.transform.position);

        Target newTarget = _targetGenerator.GenerateNewTarget();
        if (newTarget == null) return;
        
        // check if dog is on idle or action
        switch (curState)
        {
            case DogState.Idle:
                curState = DogState.Follow;
                _playerFollower.SetNextTarget(newTarget);
                _playerFollower.GoToNextTarget();
                break;
            case DogState.FollowTOI:
                _playerFollower.SetNextTarget(newTarget);
                break;
        }
        
    }

    private void HandlePlayerIdleBehavior()
    {
        curState = DogState.Idle;
    }

    private void StartWalkingAfterNextTarget()
    {
        Target newTarget = _targetGenerator.GenerateNewTarget();
        
        if (newTarget == null)
        {
            print("newTarget == null");
            return;
        }
        curState = DogState.Follow;
        _playerFollower.SetNextTarget(newTarget);
        _playerFollower.GoToNextTarget();
    }

    private void HandleDogFollowTOI()
    {
        // curState = DogState.FollowTOI;
        _dogFollowingTOI = true;
    }
    
    private void HandleDogFollow()
    {
        // curState = DogState.Follow;
        _dogFollowingTarget = true;
    }
    
    private void HandleDogIdle()
    {
        // Debug.Log("Dog is idle after finishing a target.");
        // curState = DogState.Idle;
        _dogReachedTarget = true;
    }
}