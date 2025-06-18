using System;
using System.Collections;
using System.Linq;
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
            // reset camera
            CameraController.instance.FollowPlayer();
            
            _curTarget = 0;
            TargetGenerator.instance.SetStealthTarget(_targets[0]);
            
            // reset player
            _player.SetProtected(false);
            
            // reset dog
            _dog.StealthObs(false);
            
            // // reset targets
            foreach (var target in _targets)  
            {
                target.FinishTargetAction();
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
        }

        public override void PlayerReachedTarget()
        {
            _player.SetProtected(false);
            CameraController.instance.FollowPlayer();
        }

        public void TargetReached(bool isLast)
        {
            if (!isLast) _curTarget++;
            else TargetGenerator.instance.SetStealthTarget(_lastTarget);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Dog"))
            {
                TargetGenerator.instance.SetStealthTarget(_targets[0]);
                _dog.StealthObs(true);
                CameraController.instance.FollowPlayerAndDog();
            }
        }

        private void ThrewStick(Transform stick)
        {
            if (!_areaDetector.didStickLand) return;
            
            // print("target changed to " + _targets[_curTarget].name);
            TargetGenerator.instance.SetStealthTarget(_targets[_curTarget]);
            
            GhostMovement cur = _ghosts[_curTarget-1];
            if (cur != null) cur.GoToTargetAndPause(stick);
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
