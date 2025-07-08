using System;
using Audio.FMOD;
using Dog;
using FMOD.Studio;
using UnityEngine;

public class StealthObject : MonoBehaviour
{
    private PlayerStealthManager _curStealthManager;
    private DogActionManager _curDogActionManager;
    private EventInstance _bushSound;

    [SerializeField] private bool _isStealthOut = true;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _curStealthManager = other.GetComponent<PlayerStealthManager>();
           _curStealthManager.SetStealthMode(true);
           _curStealthManager.StealthObstacle(true);
           
           if (!_bushSound.isValid()) 
               _bushSound = AudioManager.Instance.PlayLoopingSound(FMODEvents.Instance.BushRustle);
        }
        else if (other.CompareTag("Dog"))
        {
            _curDogActionManager = other.GetComponent<DogActionManager>();
            _curDogActionManager.HandleDogProtectionChanged(true);
            
            // Vector3 pos = new Vector3(transform.position.x - 1f, transform.position.y - 0.2f, 0);
            // _curDogActionManager.GetComponent<SmoothMover>().MoveTo(pos);
            
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
            _curStealthManager = null;
            
            AudioManager.Instance.StopSound(_bushSound);
            _bushSound = default;
        }
        else if (other.CompareTag("Dog"))
        {
            _curDogActionManager.HandleDogProtectionChanged(false);
            _curDogActionManager = null;
            
            AudioManager.Instance.StopSound(_bushSound);
            _bushSound = default;
        }
    }
}
