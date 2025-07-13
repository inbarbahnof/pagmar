using System;
using System.Collections;
using UnityEngine;

public class PlayerScaredOnCollision : MonoBehaviour
{
    [SerializeField] private bool _fromLeft; // true = scare if moves left, false = scare if moves right
    private PlayerMove _player;
    private PlayerStateManager _playerState;
    private Coroutine _scareRoutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_player == null)
            {
                _player = other.GetComponent<PlayerMove>();
                _playerState = _player.GetComponent<PlayerStateManager>();
            }

            if (_scareRoutine == null)
                _scareRoutine = StartCoroutine(PlayerScaredLoop());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_scareRoutine != null)
            {
                StopCoroutine(_scareRoutine);
                _scareRoutine = null;
            }
        }
    }

    private IEnumerator PlayerScaredLoop()
    {
        bool wasMovingInTriggerDirection = false;

        while (true)
        {
            float moveInputX = _player.MoveInput.x;

            bool isMovingInTriggerDirection = _fromLeft ? (moveInputX < 0) : (moveInputX > 0);

            if (isMovingInTriggerDirection && !wasMovingInTriggerDirection)
            {
                _playerState.PlayerScared();
                yield return new WaitForSeconds(0.7f); // Delay to avoid spamming
            }

            wasMovingInTriggerDirection = isMovingInTriggerDirection;
            yield return null;
        }
    }
}