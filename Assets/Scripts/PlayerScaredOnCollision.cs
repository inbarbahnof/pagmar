using System;
using System.Collections;
using UnityEngine;

public class PlayerScaredOnCollision : MonoBehaviour
{
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
        bool wasMovingRight = false;

        while (true)
        {
            bool isMovingRight = _player.MoveInput.x > 0;

            if (isMovingRight && !wasMovingRight)
            {
                _playerState.PlayerScared();
                yield return new WaitForSeconds(0.7f); // Delay to avoid spamming
            }

            wasMovingRight = isMovingRight;
            yield return null;
        }
    }
}