using UnityEngine;
using UnityEngine.Playables;

public class Lvl2CutsceneManager : MonoBehaviour
{
        [SerializeField] private PlayableDirector sequence;
        [SerializeField] private PlayerMove _playerMove;
        
        private bool _dogInPlace;
        private bool _playerInPlace;
        
        public void UpdateDogInPlace()
        {
                _dogInPlace = true;
                StartCutscene();
        }
        
        public void UpdatePlayerInPlace()
        {
                _playerInPlace = true;
                _playerMove.SetCanMove(false);
                StartCutscene();
        }

        private void StartCutscene()
        {
                if (_playerInPlace && _dogInPlace && (sequence.state != PlayState.Playing))
                {
                        sequence.Play();
                }
        }
}