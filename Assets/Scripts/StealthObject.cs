using System;
using Audio.FMOD;
using Dog;
using FMOD.Studio;
using Interactables;
using Targets;
using UnityEngine;

public class StealthObject : MonoBehaviour
{
    private PlayerStealthManager _curStealthManager;
    private DogActionManager _curDogActionManager;
    private EventInstance _bushSound;
    
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private Target _target;
    [SerializeField] private bool _isStealthOut = true;
    [SerializeField] private Stealth1Obstacle _obstacle;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _curStealthManager = other.GetComponent<PlayerStealthManager>();
           _curStealthManager.SetStealthMode(true);
           _curStealthManager.StealthObstacle(true);
           
           if (_obstacle != null && _target != null)
               _obstacle.SetStealthTarget(true, _target);
           
           _particle.Play();
           _animator.SetTrigger("enter");
           
           if (!_bushSound.isValid()) 
               _bushSound = AudioManager.Instance.PlayLoopingSound(FMODEvents.Instance.BushRustle);
        }
        else if (other.CompareTag("Dog"))
        {
            _curDogActionManager = other.GetComponent<DogActionManager>();
            _curDogActionManager.HandleDogProtectionChanged(true);
            
            _particle.Play();
            _animator.SetTrigger("enter");
            
            if (!_bushSound.isValid()) 
                _bushSound = AudioManager.Instance.PlayLoopingSound(FMODEvents.Instance.BushRustle);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _curStealthManager.SetStealthMode(false);
            if (_isStealthOut) _curStealthManager.StealthObstacle(false);
            else if (_obstacle != null && _target != null)
                _obstacle.SetStealthTarget(false, _target);
            
            _curStealthManager = null;
            _particle.Play();
            _animator.SetTrigger("enter");
            
            AudioManager.Instance.StopSound(_bushSound);
            _bushSound = default;
        }
        else if (other.CompareTag("Dog"))
        {
            _curDogActionManager.HandleDogProtectionChanged(false);
            _curDogActionManager = null;
            
            _particle.Play();
            _animator.SetTrigger("enter");
            
            AudioManager.Instance.StopSound(_bushSound);
            _bushSound = default;
        }
    }
}
