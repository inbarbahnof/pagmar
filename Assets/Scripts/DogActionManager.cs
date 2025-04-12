using System;
using UnityEngine;

public class DogActionManager : MonoBehaviour
{
    private DogState curState = DogState.Idle;

    [SerializeField] private PlayerStateManager playerStateManager;
    [SerializeField] private RandomTargetGenerator _targetGenerator;
    
    private PlayerFollower _playerFollower;

    private void Start()
    {
        _playerFollower = GetComponent<PlayerFollower>();
        
        _playerFollower.OnIdleAfterTarget += HandleDogIdle;
        _playerFollower.OnStartHover += HandleDogHover;
        _playerFollower.OnStartFollow += HandleDogFollow;

        StartWalkingAfterNextTarget();
    }

    // public void SetPlayerStateManager(PlayerStateManager player)
    // {
    //     playerStateManager = player;
    //     // playerStateManager.OnPlayerStateChange += HandlePlayerStateChange;
    // }
    
    private void Update()
    {
        switch (playerStateManager.CurrentState)
        {
            case PlayerState.Walk:
                HandlePlayerWalkBehavior();
                break;
            case PlayerState.Idle:
                HandlePlayerIdleBehavior();
                break;
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
            case DogState.Hover:
                _playerFollower.SetNextTarget(newTarget);
                break;
        }
        
    }

    private void HandlePlayerIdleBehavior()
    {
        
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

    private void HandleDogHover()
    {
        curState = DogState.Hover;
    }
    
    private void HandleDogFollow()
    {
        curState = DogState.Follow;
    }
    
    private void HandleDogIdle()
    {
        Debug.Log("Dog is idle after finishing a target.");
        curState = DogState.Idle;
    }
}