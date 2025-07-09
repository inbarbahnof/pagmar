using System.Collections;
using Audio.FMOD;
using Dog;
using Targets;
using UnityEngine;

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
        [SerializeField] private GameObject _ghost;

        public void Level1EndBehavior()
        {
            _firstTarget.FinishTargetAction();
            StartCoroutine(DogBehaviorCoruotine());
        }

        public void TriggerDogRunning()
        {
            TargetGenerator.instance.SetWantFoodTarget(_firstTarget);
            _dog.SetWantsFood(true);
            
            _dog.Running(true);
        }

        public void FreeDog()
        {
            print("free dog");
            _dog.SetWantsFood(false);
            _dog.SetMovementEnabled(true);
            _dog.ChangeCrouching(false);
            _ghost.SetActive(false);
        }

        private IEnumerator DogBehaviorCoruotine()
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.DogScared,
                _dog.transform.position, true);
            yield return new WaitForSeconds(0.5f);
            
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
            _playerMove.SetCanMove(false);

            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.DogScared,
                _dog.transform.position, true);

            yield return new WaitForSeconds(1f);
            _secondTarget.FinishTargetAction();
            _dog.SetMovementEnabled(false);

            _cutsceneManager.ShowSequence();
        }
    }
}