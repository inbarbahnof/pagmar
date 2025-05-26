using System;
using System.Collections;
using Dog;
using Ghosts;
using Targets;
using UnityEngine;

namespace Interactables
{
    public class DogStealthDistractionObsManager : Obstacle
    {
        [SerializeField] private DogActionManager _dog;
        [SerializeField] private Target[] _targets;
        [SerializeField] private GhostMovement[] _ghosts;
        [SerializeField] private ThrowablePickUpInteractable[] _sticks;

        private int _curTarget = 0;

        private void Start()
        {
            foreach (var stick in _sticks)
            {
                stick.OnThrowComplete += ThrewStick;
            }

            StartCoroutine(WaitToSetTarget());
        }

        private IEnumerator WaitToSetTarget()
        {
            yield return new WaitForSeconds(0.1f);
            TargetGenerator.instance.SetStealthTarget(_targets[0]);
        }

        public override void ResetObstacle()
        {
            _curTarget = 0;
            TargetGenerator.instance.SetStealthTarget(_targets[0]);
            
            // reset sticks positions

            // reset ghost positions
            foreach (var ghost in _ghosts)  
            {
                ghost.MoveAround();
            }
        }

        public void TargetReached()
        {
            print("target reached");
            _curTarget++;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Dog"))
            {
                _dog.StealthObs();
            }
        }

        private void ThrewStick(Transform stick)
        {
            print("target changed to " + _targets[_curTarget].name);
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
