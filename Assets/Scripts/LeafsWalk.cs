using System;
using UnityEngine;

public class LeafsWalk : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particle;
    private PlayerAnimationManager _player;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_player == null) _player = other.GetComponent<PlayerAnimationManager>();
            _player.SetLeafsParticles(_particle);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_player == null) _player = other.GetComponent<PlayerAnimationManager>();
            _player.SetLeafsParticles(null);
        }
    }
}
