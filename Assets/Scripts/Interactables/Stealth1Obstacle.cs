using System;
using Targets;
using UnityEngine;

namespace Interactables
{ 
    public class Stealth1Obstacle : Obstacle // Start is called once before the first execution of Update after the MonoBehaviour is created
    {
        [SerializeField] private GameObject _ghost;
        [SerializeField] private Target _stealthTarget;

        private bool _didGhostAppear;
        
        public void GhostAppear()
        {
            // TODO make noise and bring the ghost
            
            if (_ghost != null)  _ghost.SetActive(true);
            TargetGenerator.instance.SetStealthTarget(_stealthTarget);

            _didGhostAppear = true;
        }

        public override void ResetObstacle()
        {
            // CameraController.instance.FollowPlayer();
            _stealthTarget.FinishTargetAction();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_didGhostAppear && other.CompareTag("Dog"))
            {
                CameraController.instance.FollowPlayerAndDog();
            }
        }

        public override void PlayerReachedTarget()
        {
            CameraController.instance.FollowPlayer();
        }
    }
}

