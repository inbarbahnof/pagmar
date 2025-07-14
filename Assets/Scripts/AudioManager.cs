using System;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;
using System.Collections;

namespace Audio.FMOD
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        
        private List<EventInstance> _eventInstances;
        private List<StudioEventEmitter> _eventEmitters;
        
        public EventInstance musicInstance;
        private EventInstance ambianceInstance;

        private EventInstance ambienceMute;
        private EventInstance musicMute;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            _eventInstances = new List<EventInstance>();
            _eventEmitters = new List<StudioEventEmitter>();

            if (!ambienceMute.isValid())
                InitializeSnapshots(FMODEvents.Instance.MuteAmbienceSnapshot, 0);
            if (!musicMute.isValid())
                InitializeSnapshots(FMODEvents.Instance.MuteMusicSnapshot, 1);


        }

        /// <summary>
        /// 0 = Ambience, 1 = Music
        /// </summary>
        /// <param name="eventReference"></param>
        /// <param name="musicOrAmbience"></param>
        private void InitializeSnapshots(EventReference eventReference, int musicOrAmbience)
        {
            if (!eventReference.IsNull)
            {
                switch (musicOrAmbience)
                {
                    case 0: // Ambience
                        ambienceMute = RuntimeManager.CreateInstance(eventReference);
                        break;
                    case 1: // Music
                        musicMute = RuntimeManager.CreateInstance(eventReference);
                        break;
                    default:
                        Debug.LogWarning("Invalid musicOrAmbience value. Use 0 for Ambience and 1 for Music.");
                        break;
                }
               
            }
            else
                Debug.LogWarning("EventReference for snapshots is null.");

        }

        public void PlayMusic(EventReference music)
        {
            if (musicInstance.isValid())
            {
                musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
                musicInstance.release();
            }

            musicInstance = RuntimeManager.CreateInstance(music);
            musicInstance.start();
        }

        public void StopMusic()
        {
            if (musicInstance.isValid())
            {
                musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
                musicInstance.release();
            }
        }

        public void PlayAmbiance(EventReference music)
        {
            if (ambianceInstance.isValid())
            {
                ambianceInstance.stop(STOP_MODE.ALLOWFADEOUT);
                ambianceInstance.release();
            }
            
            ambianceInstance = RuntimeManager.CreateInstance(music);

            /// itamar added this --- to have the ambiance sound follow the camera position and have ATTENUATION
            ambianceInstance.set3DAttributes(Camera.main.transform.position.To3DAttributes()); 

            ambianceInstance.start();
        }
        
        public void PlayOneShot(EventReference sound, Vector3 pos = default, bool useDirection = false)
        {
            EventInstance instance = RuntimeManager.CreateInstance(sound);
            if (useDirection)
            {
                // Play 3D sound with a fixed position
                instance.set3DAttributes(pos.To3DAttributes());
                // RuntimeManager.PlayOneShot(sound, pos);
            }

            instance.start();
            instance.release();
        }
        
        public EventInstance PlayLoopingSound(EventReference sound, Vector3 pos = default, bool useDirection = false)
        {
            EventInstance instance = RuntimeManager.CreateInstance(sound);
    
            if (useDirection)
            {
                instance.set3DAttributes(pos.To3DAttributes());
            }

            instance.start();  // Starts playing the sound
            return instance;   // Return the instance to allow stopping later
        }
        
        public void StopSound(EventInstance instance)
        {
            instance.stop(STOP_MODE.ALLOWFADEOUT);  // Stops the sound gracefully
            instance.release();  // Releases the instance when done
        }

        public StudioEventEmitter InitializeEvenEmitter(EventReference eventReference, GameObject emitterGameObject)
        {
            StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
            emitter.EventReference = eventReference;
            _eventEmitters.Add(emitter);
            return emitter;
        }
        
        public void SetStringParameter(EventInstance emitter, string parameterName, string parameterValue, bool isGlobal)
        {
            if (isGlobal)
            {
                RuntimeManager.StudioSystem.setParameterByNameWithLabel(parameterName, parameterValue);
            }
            else
            {
                emitter.setParameterByNameWithLabel(parameterName, parameterValue);
            }
        }
        
        public void SetFloatParameter(EventInstance emitter, string parameterName, float parameterValue, bool isGlobal)
        {
            // print("setting parameter " + parameterName + " to " + parameterValue);
            if (isGlobal)
            {
                RuntimeManager.StudioSystem.setParameterByName(parameterName, parameterValue);
            }
            else
            {
                emitter.setParameterByName(parameterName, parameterValue);
            }
        }
        
        public void PlaySoundWithParameter(EventReference sound, Vector3 pos, string parameterName, float parameterValue)
        {
            // Create an EventInstance
            var eventInstance = RuntimeManager.CreateInstance(sound);

            // Set 3D position
            eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(pos));

            // Set parameter before starting
            eventInstance.setParameterByName(parameterName, parameterValue);
 
            // Start playing
            eventInstance.start();

            // Release instance once done (to avoid memory leaks)
            eventInstance.release();
        }
        
        public void PlaySoundWithStringParameter(EventReference sound, Vector3 pos, string parameterName, string parameterValue)
        {
            // Create an EventInstance
            var eventInstance = RuntimeManager.CreateInstance(sound);

            // Set 3D position
            eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(pos));

            // Set parameter before starting
            eventInstance.setParameterByNameWithLabel(parameterName, parameterValue);
 
            // Start playing
            eventInstance.start();

            // Release instance once done (to avoid memory leaks)
            eventInstance.release();
        }

        public void MuteAmbienceEvent()
        {
            if (ambianceInstance.isValid())
                StartCoroutine(PauseEventCoroutine(0));
        }

        public void MuteMusicEvent()
        {
            if (musicInstance.isValid())
                StartCoroutine(PauseEventCoroutine(1));
        }
        
        public void ResumeMusic()
        {
            if (musicInstance.isValid())
            {
                ResumeEvent(1);
            }
        }

        public void ResumeAmbience()
        {
            if (ambianceInstance.isValid())
            {
                ResumeEvent(0);
            }
        }


        /// <summary>
        /// 0 = ambience, 1 = music
        /// </summary>
        /// <param name="ambienceOrMusic"></param>
        /// <returns></returns>
        private IEnumerator PauseEventCoroutine(int ambienceOrMusic)
        {
            switch (ambienceOrMusic)
            {
                case 0: // Ambience
                    ambienceMute.start();

                    yield return new WaitForSeconds(1f); // Wait for a short duration to ensure the event is paused
                    ambianceInstance.setPaused(true);
                    break;

                case 1: // Music
                    musicMute.start();
                    yield return new WaitForSeconds(1f); // Wait for a short duration to ensure the event is paused
                    musicInstance.setPaused(true);
                    break;

            }
        }

        private void ResumeEvent(int ambienceOrMusic)
        {
            switch (ambienceOrMusic)
            {
                case 0: // Ambience
                    ambianceInstance.setPaused(false);
                    ambienceMute.stop(STOP_MODE.ALLOWFADEOUT);
                    break;

                case 1: // Music
                    musicInstance.setPaused(false);
                    musicMute.stop(STOP_MODE.ALLOWFADEOUT);
                    break;
            }
        }

      

        public EventInstance CreateEventInstance(EventReference eventReference)
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
            _eventInstances.Add(eventInstance);
            return eventInstance;
        }
        
        private void Cleanup() {
            foreach (EventInstance eventInstance in _eventInstances)
            {
                eventInstance.stop(STOP_MODE.IMMEDIATE);
            }

            foreach (StudioEventEmitter emitter in _eventEmitters)
            {
                emitter.Stop();
            }
        }

        private void OnDisable()
        {
            Cleanup();
        }
    }
}