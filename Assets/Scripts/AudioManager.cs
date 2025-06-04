using System;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Audio.FMOD
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        
        private List<EventInstance> _eventInstances;
        private List<StudioEventEmitter> _eventEmitters;
        
        private EventInstance musicInstance;
        private EventInstance ambianceInstance;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            _eventInstances = new List<EventInstance>();
            _eventEmitters = new List<StudioEventEmitter>();
        }
        
        public void PlayMusic(EventReference music)
        {
            musicInstance.stop(STOP_MODE.IMMEDIATE);
            
            musicInstance = RuntimeManager.CreateInstance(music);
            musicInstance.start();
        }

        public void PlayAmbiance(EventReference music)
        {
            ambianceInstance = RuntimeManager.CreateInstance(music);
            ambianceInstance.start();
        }
        
        public void PlayOneShot(EventReference sound, Vector3 pos, bool useDirection)
        {
            if (useDirection)
            {
                // Play 3D sound with a fixed position
                RuntimeManager.PlayOneShot(sound, pos);
            }
            else
            {
                // Play 2D sound
                RuntimeManager.PlayOneShot(sound);
            }
        }
        
        public EventInstance PlayLoopingSound(EventReference sound, Vector3 pos, bool useDirection)
        {
            EventInstance instance = RuntimeManager.CreateInstance(sound);
    
            if (useDirection)
            {
                instance.set3DAttributes(RuntimeUtils.To3DAttributes(pos));
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
        
        public void SetParameter(EventInstance emitter, string parameterName, float parameterValue, bool isGlobal)
        {
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