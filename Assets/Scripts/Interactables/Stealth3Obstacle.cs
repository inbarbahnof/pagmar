using System;
using System.Collections;
using System.Linq;
using Audio.FMOD;
using Dog;
using Ghosts;
using Targets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactables
{
    public class Stealth3Obstacle : Obstacle
    {
        [SerializeField] private DogActionManager _dog;
        [SerializeField] private PlayerStealthManager _player;
        [SerializeField] private StickAreaDetector _areaDetector;
        
        [Header("Dog Stealth")]
        [SerializeField] private Target _lastTarget;
        [SerializeField] private Target[] _targets;
        [SerializeField] private GhostMovement[] _ghostsForDog;
        
        [Header("Player Stealth")]
        [SerializeField] private GhostMovement[] _ghostsForPlayer;
        [SerializeField] private Transform[] _ghostDistractionForPlayer;
        
        [Header("Sticks")]
        [SerializeField] private ThrowablePickUpInteractable[] _sticks;

        private int _curDogTarget = 0;
        private int _curPlayerBush = 0;
        private Vector3[] _stickPositions;
        
        private bool _dogReachedFirstTarget;
        private bool _playerReachedCurrentTarget = false;
        private Coroutine _dogDistraction;

        private void Start()
        {
            _stickPositions = new Vector3[_sticks.Length];
              
            for (int i = 0; i < _sticks.Length; i++)
            {
                _stickPositions[i] = _sticks[i].transform.position;
                _sticks[i].OnThrowComplete += ThrewStick;
            }

            StartCoroutine(WaitToSetTarget());
        }

        private IEnumerator WaitToSetTarget()
        {
            yield return new WaitForSeconds(0.5f);
            TargetGenerator.instance.SetStealthTarget(_targets[0]);
        }

        public override void ResetObstacle()
        {
            PickUpInteractableManager.instance.DropObject();
            
            // reset sticks positions
            for (int i = 0; i < _stickPositions.Length; i++)
            {
                _sticks[i].transform.position = _stickPositions[i];
            }

            // reset ghost positions
            foreach (var ghost in _ghostsForDog)  
            {
                ghost.MoveAround();
            }
            
            // reset ghost positions
            foreach (var ghost in _ghostsForPlayer)  
            {
                ghost.MoveAround();
            }
            
            _curDogTarget = 0;
            _curPlayerBush = 0;
            _playerReachedCurrentTarget = true;
            _dogReachedFirstTarget = false;
            TargetGenerator.instance.SetStealthTarget(_targets[0]);
            
            _dog.StealthObs(true);
            CameraController.instance.FollowPlayerAndDog();

            StartCoroutine(WaitToDogProtected());
        }

        private IEnumerator WaitToDogProtected()
        {
            yield return new WaitForSeconds(2f);
            _dog.HandleDogProtectionChanged(true);
            _player.SetProtected(true);
        }

        public void PlayerReachedStealth()
        {
            _playerReachedCurrentTarget = false;
            
            if (_dogDistraction != null) StopCoroutine(_dogDistraction);
            _dogDistraction = StartCoroutine(WaitForDogBark());
        }

        private IEnumerator WaitForDogBark()
        {
            yield return new WaitUntil(() => _dogReachedFirstTarget);
            
            while (!_playerReachedCurrentTarget)
            {
                _dog.Bark();

                if (_curPlayerBush < _ghostsForPlayer.Length)
                {
                    _ghostsForPlayer[_curPlayerBush].GoToTargetAndPause(_ghostDistractionForPlayer[_curPlayerBush]);
                }

                yield return new WaitForSeconds(5f);
            }

            _curPlayerBush++;
            _playerReachedCurrentTarget = false;
        }

        public void PlayerReachedNextTarget()
        {
            _playerReachedCurrentTarget = true;
            _dogReachedFirstTarget = false;
        }

        public override void PlayerReachedTarget()
        {
            CameraController.instance.FollowPlayer();
            _dog.ChangeCrouching(false);
            _player.StealthObstacle(false);
            
            AudioManager.Instance.SetFloatParameter(default,
                "Stealth Echo",
                0,
                true);
        }

        public void TargetReached(bool isLast)
        {
            _dogReachedFirstTarget = true;
            if (!isLast) _curDogTarget++;
            else TargetGenerator.instance.SetStealthTarget(_lastTarget);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Dog"))
            {
                TargetGenerator.instance.SetStealthTarget(_targets[0]);
                _dog.StealthObs(true);
                CameraController.instance.FollowPlayerAndDog();
                _player.StealthObstacle(true);
                
                AudioManager.Instance.SetFloatParameter(default,
                    "Stealth Echo",
                    1,
                    true);
            }
        }

        public void DogCrouch()
        {
            _dog.ChangeCrouching(true);
        }
        
        private void ThrewStick(Transform stick)
        {
            if (!_areaDetector.didStickLand) return;
            
            // print("target changed to " + _targets[_curTarget].name);
            GhostMovement cur = _ghostsForDog[_curDogTarget-1];
            if (cur != null) cur.GoToTargetAndPause(stick);
            
            StartCoroutine(WaitToChangeTarget());
        }

        private IEnumerator WaitToChangeTarget()
        {
            yield return new WaitForSeconds(0.5f);
            TargetGenerator.instance.SetStealthTarget(_targets[_curDogTarget]);
        }
        
        private void OnDestroy()
        {
            foreach (var stick in _sticks)
            {
                stick.OnThrowComplete -= ThrewStick;
            }
        }
    }
}
