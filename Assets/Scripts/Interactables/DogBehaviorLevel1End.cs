using System.Collections;
using Audio.FMOD;
using Dog;
using Targets;
using UnityEngine;

namespace Interactables
{
    public class DogBehaviorLevel1End : MonoBehaviour
    {
        [SerializeField] private DogWaitForPlayer _dogWait;
        [SerializeField] private WantFoodTarget _secondTarget;
        [SerializeField] private DogActionManager _dog;

        public void Level1EndBehavior()
        {
            StartCoroutine(DogBehaviorCoruotine());
        }

        private IEnumerator DogBehaviorCoruotine()
        {
            // AudioManager.Instance.PlayOneShot(FMODEvents.Instance.DogScared);
            yield return new WaitForSeconds(0.4f);
            
            _secondTarget.gameObject.SetActive(true);
            TargetGenerator.instance.SetWantFoodTarget(_secondTarget);
            
            yield return new WaitForSeconds(0.7f);
            
            _dog.SetWantsFood(true);
            _dog.Running(true);
        }

        public void PlayDogScared()
        {
            _dog.ChangeCrouching(true);
        }
    }
}