using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerNarrowEntrance : MonoBehaviour
{
    private PlayerStateManager _stateManager;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_stateManager == null)
                _stateManager = other.GetComponent<PlayerStateManager>();
            _stateManager.OnNarrowPass(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_stateManager == null)
                _stateManager = other.GetComponent<PlayerStateManager>();
            _stateManager.OnNarrowPass(false);
        }
    }
}
