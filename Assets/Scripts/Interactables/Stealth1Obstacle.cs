using System;
using System.Collections;
using Audio.FMOD;
using Dog;
using Ghosts;
using Targets;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactables
{ 
    public class Stealth1Obstacle : Obstacle // Start is called once before the first execution of Update after the MonoBehaviour is created
    {
        [FormerlySerializedAs("_ghost")] [SerializeField] private GameObject _ghostGameObject;
        [SerializeField] private GhostMovement _ghostMovement;
        [SerializeField] private Target _stealthTarget;
        [SerializeField] private PlayerStealthManager _player;
        [SerializeField] private DogActionManager _dog;

        private bool _didGhostAppear;
        private Coroutine _coroutine;
        
        public void GhostAppear(Transform stick)
        {
            if (_ghostGameObject != null)
            {
                AudioManager.Instance.MuteMusicEvent();
                
                _ghostGameObject.SetActive(true);
                _ghostMovement.GoToTargetAndPause(stick);
                
                _player.SetProtected(true);
                _dog.HandleDogProtectionChanged(true);
            }
            
            TargetGenerator.instance.SetStealthTarget(_stealthTarget);

            _didGhostAppear = true;

            if (_coroutine == null)
                _coroutine = StartCoroutine(WaitToGhostCome());
        }

        private IEnumerator WaitToGhostCome()
        {
            yield return new WaitForSeconds(7f);
            
            AudioManager.Instance.ResumeMusic();
            _player.SetProtected(false);
            _dog.HandleDogProtectionChanged(false);
        }

        public void SetStealthTarget(bool isEntered, Target target)
        {
            if (isEntered) TargetGenerator.instance.SetStealthTarget(target);
            else TargetGenerator.instance.SetStealthTarget(_stealthTarget);
        }

        public override void ResetObstacle()
        {
            // CameraController.instance.ZoomIn();
            _stealthTarget.FinishTargetAction();
            
            _dog.ChangeCrouching(true);
            // _player.StealthObstacle(false);

            StartCoroutine(WaitToProtectPlayer());
        }

        private IEnumerator WaitToProtectPlayer()
        {
            yield return new WaitForSeconds(1f);
            _player.SetProtected(true);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_didGhostAppear && other.CompareTag("Player"))
            {
                CameraController.instance.ZoomOut();
                _player.StealthObstacle(true);
                _dog.ChangeCrouching(true);
                
                AudioManager.Instance.SetFloatParameter(default,
                    "Stealth Echo",
                    1,
                    true);
            }
        }

        public override void PlayerReachedTarget()
        {
            CameraController.instance.FollowPlayer();
            _player.StealthObstacle(false);
            
            AudioManager.Instance.SetFloatParameter(default,
                "Stealth Echo",
                0,
                true);
        }
    }
}

