using UnityEngine;
using FMODUnity;

namespace Audio.FMOD
{
    public class FMODEvents : MonoBehaviour
    {
        [field: Header("GeneralSFX")]
        [field: SerializeField] public EventReference ObjectFall { get; private set; }
        [field: SerializeField] public EventReference BushRustle { get; private set; }
        [field: SerializeField] public EventReference GhostieSound { get; private set; }
        [field: SerializeField] public EventReference GhostieBonesDeath { get; private set; }
        [field: SerializeField] public EventReference GhostSound { get; private set; }
        [field: SerializeField] public EventReference DropStealthObject { get; private set; }
        [field: SerializeField] public EventReference CageFall { get; private set; }
        [field: SerializeField] public EventReference CageSnap { get; private set; }
        
        [field: Header("PlayerSFX")]
        [field: SerializeField] public EventReference PlayerCall { get; private set; }
        [field: SerializeField] public EventReference PlayerFootsteps { get; private set; } 
        [field: SerializeField] public EventReference PlayerPickUp{ get; private set; }
        [field: SerializeField] public EventReference PlayerThrow { get; private set; }
        [field: SerializeField] public EventReference PlayerDrag { get; private set; }
        [field: SerializeField] public EventReference PlayerSigh { get; private set; }
        [field: SerializeField] public EventReference PlayerScared { get; private set; }
        [field: SerializeField] public EventReference PlayerDeath { get; private set; }
        
        [field: Header("DogSFX")]
        [field: SerializeField] public EventReference DogBark { get; private set; }
        [field: SerializeField] public EventReference DogFootsteps { get; private set; }
        [field: SerializeField] public EventReference DogGrowl{ get; private set; }
        [field: SerializeField] public EventReference DogSniff{ get; private set; }
        [field: SerializeField] public EventReference DogScared{ get; private set; }
        [field: SerializeField] public EventReference DogEat{ get; private set; }
        [field: SerializeField] public EventReference DogDie{ get; private set; }
        [field: SerializeField] public EventReference DogHappyCrouch{ get; private set; }

        [field: Header("Music")]
        [field: SerializeField] public EventReference Chapter0Music { get; private set; }
        [field: SerializeField] public EventReference Chapter0Cutscene { get; private set; }
        [field: SerializeField] public EventReference Chapter1Music { get; private set; }
        [field: SerializeField] public EventReference Chapter2Music { get; private set; }
        [field: SerializeField] public EventReference Chapter3Music { get; private set; }
        [field: SerializeField] public EventReference Chapter3ReunionMusic { get; private set; }
        [field: SerializeField] public EventReference Chapter4Music { get; private set; }
        [field: SerializeField] public EventReference ChapterRunning4Music { get; private set; }

        [field: Header("Ambience")]
        [field: SerializeField] public EventReference Ambiance { get; private set; }


        [field: Header("Snapshots")]
        [field: SerializeField] public EventReference MuteAmbienceSnapshot { get; private set; }
        [field: SerializeField] public EventReference MuteMusicSnapshot { get; private set; }



        public static FMODEvents Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}