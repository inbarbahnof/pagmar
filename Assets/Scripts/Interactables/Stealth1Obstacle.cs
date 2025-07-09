using System;
using Audio.FMOD;
using Dog;
using Targets;
using UnityEngine;

namespace Interactables
{ 
    public class Stealth1Obstacle : Obstacle // Start is called once before the first execution of Update after the MonoBehaviour is created
    {
        [SerializeField] private GameObject _ghost;
        [SerializeField] private Target _stealthTarget;
        [SerializeField] private PlayerStealthManager _player;
        [SerializeField] private DogActionManager _dog;

        private bool _didGhostAppear;
        
        public void GhostAppear()
        {
            // TODO make noise and bring the ghost
            
            if (_ghost != null)  _ghost.SetActive(true);
            TargetGenerator.instance.SetStealthTarget(_stealthTarget);

            _didGhostAppear = true;
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
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_didGhostAppear && other.CompareTag("Player"))
            {
                CameraController.instance.ZoomOut();
                _player.StealthObstacle(true);
                _dog.ChangeCrouching(true);
            }
        }

        public override void PlayerReachedTarget()
        {
            CameraController.instance.FollowPlayer();
            _player.StealthObstacle(false);
        }
    }
}

