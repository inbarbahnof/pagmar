using Audio.FMOD;
using FMOD.Studio;
using UnityEngine;

namespace Ghosts
{
    public class PlayGhostieSound : MonoBehaviour
    {
        private bool _hasTriggered = false;
        private EventInstance _ghostieSound; 
        
        private void OnBecameVisible()
        {
            if (!_hasTriggered)
            {
                _hasTriggered = true;
                _ghostieSound = AudioManager.Instance.PlayLoopingSound(FMODEvents.Instance.GhostieSound,
                    transform.position, true);
            }
        }

        private void OnBecameInvisible()
        {
            if (_ghostieSound.isValid())
            {
                AudioManager.Instance.StopSound(_ghostieSound);
                _hasTriggered = false;
            }
        }
    }
}