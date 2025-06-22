using Audio.FMOD;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ghosts
{
    public class PlayGhostieSound : MonoBehaviour
    {
        [SerializeField]float maxPlayableDistance = 20f;
        private EventInstance _ghostieSound;
        private bool hasStarted;

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

        void Update()
        {
            if (!hasStarted && Vector3.Distance(transform.position, Camera.main.transform.position) < maxPlayableDistance)
            {
                _ghostieSound.start();
                hasStarted = true;
            }

            // Optional: stop it if it leaves range
            else if (hasStarted && Vector3.Distance(transform.position, Camera.main.transform.position) > maxPlayableDistance + 5f)
            {
                _ghostieSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                hasStarted = false;
            }
        }
    }
}