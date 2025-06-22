using Audio.FMOD;
using FMOD.Studio;
using UnityEngine;

namespace Ghosts
{
    public class PlayGhostieSound : MonoBehaviour
    {
        private bool _hasTriggered = true;
        private EventInstance _ghostieSound;

        private void Start()
        {
            _ghostieSound = AudioManager.Instance.PlayLoopingSound(FMODEvents.Instance.GhostieSound,
                   transform.position, true);

            // This tells FMOD to track the transform and Rigidbody
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(
                _ghostieSound,
                transform, // the Ghostie's transform
                GetComponent<Rigidbody2D>() // optional, used for velocity tracking
            );

            
        }

        private void OnBecameVisible()
        {

            /*if (!_hasTriggered)
            {
                _hasTriggered = true;
                _ghostieSound = AudioManager.Instance.PlayLoopingSound(FMODEvents.Instance.GhostieSound,
                    transform.position, true);
            }*/
        }

        private void OnBecameInvisible()
        {
           /* if (_ghostieSound.isValid())
            {
                Debug.Log("pausing ghostie sound");

                AudioManager.Instance.StopSound(_ghostieSound);
                _hasTriggered = false;
            }*/
        }
    }
}