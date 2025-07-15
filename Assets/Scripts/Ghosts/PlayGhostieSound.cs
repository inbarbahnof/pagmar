using Audio.FMOD;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ghosts
{
    public class PlayGhostieSound : MonoBehaviour
    {
        [SerializeField] float maxPlayableDistance = 20f;
        [SerializeField] private bool _isGhost;
        private EventInstance _charachterSound;
        private bool hasStarted;
        private bool dead;

        private void Start()
        {
            if (!_isGhost) _charachterSound = AudioManager.Instance.PlayLoopingSound(
                FMODEvents.Instance.GhostieSound,
                transform.position, true);
            else _charachterSound = AudioManager.Instance.PlayLoopingSound(
                FMODEvents.Instance.GhostSound,
                transform.position, true);


            // This tells FMOD to track the transform and Rigidbody
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(
                _charachterSound,
                transform, // the Ghostie's transform
                GetComponent<Rigidbody2D>() // optional, used for velocity tracking
            );
        }

        private void Update()
        {
            if (!dead && !hasStarted && 
                Vector3.Distance(transform.position, Camera.main.transform.position) < maxPlayableDistance)
            {
                _charachterSound.start();
                hasStarted = true;
            }
            else if (hasStarted && 
                     Vector3.Distance(transform.position, Camera.main.transform.position) > maxPlayableDistance + 5f)
            {
                _charachterSound.stop(STOP_MODE.ALLOWFADEOUT);
                hasStarted = false;
            }
        
        }

        private void OnDisable()
        {
            if (_charachterSound.isValid())
            {
                _charachterSound.stop(STOP_MODE.ALLOWFADEOUT);
            }

            hasStarted = false;
        }
        
        private void OnEnable()
        {
            hasStarted = false;
        }

        public void ResetGhostEndParameter()
        {
            AudioManager.Instance.SetFloatParameter(_charachterSound,
                "Ending Dog Death",
                0, false);
        }

        public void GhostEndParameter()
        {
            AudioManager.Instance.SetFloatParameter(_charachterSound,
                "Ending Dog Death",
                1, false);
        }

        public void StopGhostieSound()
        {
            if (hasStarted)
            {
                _charachterSound.setParameterByName("Ghostie Mode", 1);

                AudioManager.Instance.PlayOneShot(
                    FMODEvents.Instance.GhostieBonesDeath, 
                    transform.position, true);

                //_charachterSound.stop(STOP_MODE.ALLOWFADEOUT);
                hasStarted = false;
                dead = true;
            }
        }

        public void ResumeGhostieSound()
        {
            if (!hasStarted)
            {
                _charachterSound.start();
                hasStarted = true;
                dead = false;
            }
        }
    }
}