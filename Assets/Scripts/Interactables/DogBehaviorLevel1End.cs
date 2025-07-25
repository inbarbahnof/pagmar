using System.Collections;
using Audio.FMOD;
using Dog;
using Targets;
using UnityEngine;
using Input = UnityEngine.Windows.Input;

namespace Interactables
{
    public class DogBehaviorLevel1End : MonoBehaviour
    {
        // [SerializeField] private DogWaitForPlayer _dogWait;
        [SerializeField] private WantFoodTarget _firstTarget;
        [SerializeField] private WantFoodTarget _secondTarget;
        [SerializeField] private DogActionManager _dog;
        [SerializeField] private PlayerMove _playerMove;
        [SerializeField] private StartCutsceneManager _cutsceneManager;
        [SerializeField] private InputManager _playerInput;
        
        public void Level1EndBehavior()
        {
            _firstTarget.FinishTargetAction();
            StartCoroutine(DogBehaviorCoruotine());
        }

        public void TriggerDogRunning()
        {
            TargetGenerator.instance.SetWantFoodTarget(_firstTarget);
            _dog.SetWantsFood(true);
            _dog.FoodNotClose();
            _dog.Running(true);
        }

        public void FreeDog()
        {
            _dog.SetWantsFood(false);
            _dog.SetMovementEnabled(true);
            _dog.ChangeCrouching(false);
        }

        private IEnumerator DogBehaviorCoruotine()
        {
            AudioManager.Instance.MuteAmbienceEvent();
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.DogScared,
                _dog.transform.position, true);
            // yield return new WaitForSeconds(0.1f);
            
            _secondTarget.gameObject.SetActive(true);
            TargetGenerator.instance.SetWantFoodTarget(_secondTarget);
            
            yield return new WaitForSeconds(0.2f);
            
            _dog.SetWantsFood(true);
            _dog.Running(true);
        }

        public void PlayDogScared()
        {
            _dog.ChangeCrouching(true);
        }

        public void PlayCutscene()
        {
            StartCoroutine(StartCutscene());
        }

        private IEnumerator StartCutscene()
        {
            _playerInput.PlayerDisableAllInput();
            _playerMove.SetCanMove(false);

            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.DogScared,
                _dog.transform.position, true);

            yield return new WaitForSeconds(1f);
            _secondTarget.FinishTargetAction();
            _dog.SetMovementEnabled(false);

            _cutsceneManager.ShowSequence();

            yield return new WaitForSeconds(6f);
            AudioManager.Instance.ResumeMusic();

        }
    }
}