using System;
using System.Collections;
using System.Linq;
using Audio.FMOD;
using Dog;
using Ghosts;
using Targets;
using Unity.VisualScripting;
using UnityEngine;

namespace Interactables
{
    public class Stealth2Obstacle : Obstacle
    {
        [SerializeField] private DogActionManager _dog;
        [SerializeField] private PlayerStealthManager _player;
        [SerializeField] private StickAreaDetector _areaDetector;
        [SerializeField] private Target _lastTarget;
        [SerializeField] private Target[] _targets;
        [SerializeField] private GhostMovement[] _ghosts;
        [SerializeField] private ThrowablePickUpInteractable[] _sticks;

        private int _curTarget = 0;
        private Vector3[] _stickPositions;
        private PlayerStateManager _playerStateManager;

        private void Start()
        {
            _playerStateManager = _player.GetComponent<PlayerStateManager>();
            
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
            PickUpInteractableManager.instance.DropObject(_player.transform.position);
            
            // reset targets
            for (int i = 1; i < _targets.Length; i++)
            {
                _targets[i].FinishTargetAction();
            }
            
            // reset sticks positions
            for (int i = 0; i < _stickPositions.Length; i++)
            {
                _sticks[i].transform.position = _stickPositions[i];
            }

            // reset ghost positions
            foreach (var ghost in _ghosts)  
            {
                ghost.MoveAround();
            }
            
            _curTarget = 0;
            TargetGenerator.instance.SetStealthTarget(_targets[0]);
            _dog.StealthObs(true);
            CameraController.instance.FollowPlayerAndDog();
            
            StartCoroutine(WaitToProtectPlayer());
        }

        private IEnumerator WaitToProtectPlayer()
        {
            yield return new WaitForSeconds(1f);
            _player.SetProtected(true);
        }

        public override void PlayerReachedTarget()
        {
            _player.SetProtected(false);
            
            CameraController.instance.FollowPlayer();
            _dog.ChangeCrouching(false);
        }

        public void TargetReached(bool isLast)
        {
            if (!isLast) _curTarget++;
            else
            {
                TargetGenerator.instance.SetStealthTarget(_lastTarget);
                StartCoroutine(ChangeDogCrouch());
                
                AudioManager.Instance.SetFloatParameter(default,
                    "Stealth Echo",
                    0,
                    true);
            }
        }

        private IEnumerator ChangeDogCrouch()
        {
            yield return new WaitForSeconds(0.7f);
            _dog.ChangeCrouching(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Dog"))
            {
                TargetGenerator.instance.SetStealthTarget(_targets[0]);
                _dog.StealthObs(true);
                CameraController.instance.FollowPlayerAndDog();
                
                AudioManager.Instance.SetFloatParameter(default,
                    "Stealth Echo",
                    1,
                    true);
            }
        }

        private void ThrewStick(Transform stick)
        {
            if (!_areaDetector.didStickLand) return;
            
            // print("target changed to " + _targets[_curTarget].name);
            GhostMovement cur = _ghosts[_curTarget-1];
            if (cur != null) cur.GoToTargetAndPause(stick);
            
            StartCoroutine(WaitToChangeTarget());
        }

        private IEnumerator WaitToChangeTarget()
        {
            yield return new WaitForSeconds(0.5f);
            
            TargetGenerator.instance.SetStealthTarget(_targets[_curTarget]);
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