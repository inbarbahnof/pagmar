using System;
using UnityEngine;

public class StopPlayerToCallDog : MonoBehaviour
{
    private PlayerStateManager _player;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_player == null) 
                _player = other.gameObject.GetComponent<PlayerStateManager>();
            
            _player.StartedCalling();
        }
    }
}
