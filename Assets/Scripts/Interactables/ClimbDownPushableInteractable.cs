using System;
using System.Collections;
using UnityEngine;

namespace Interactables
{
    public class ClimbDownPushableInteractable : ClimbPushableInteractable
    {
        private bool _canClimb = false;
        private bool _climbing = false;
        private PlayerMove.Movedir _obsDir = PlayerMove.Movedir.None;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                InteractableManager.instance.AddInteractableObj(this);
                //print("triggerEnter");
            }
        }
        
        public override void Interact()
        {
            return;
        }

        public override void SetHighlight(bool isHighlighted)
        {
            if (_obsDir == PlayerMove.Movedir.None) _obsDir = climbRight ? PlayerMove.Movedir.Right : PlayerMove.Movedir.Left;
            _canClimb = isHighlighted;
            //print("highlight " + isHighlighted + name);
        }

        private void ClimbDown()
        {
            _interacting = true;
            
            if (twinTrigger is null && twin is not null) twinTrigger = twin.GetColGO();
            ClimbInteractableManager.instance.Climb(this, playerPos.position, _isClimbing, climbRight);
            if (col)
            {
                col.SetActive(false);
                //print("col deactivated at interact");
            }
            StopInteractPress();
        }
        
        public override void StopInteractPress()
        {
            if (_waitToStopInteractCoroutine is not null) return;
            _waitToStopInteractCoroutine = StartCoroutine(base.WaitToStopInteract());
        }


        public override void FinishInteraction()
        {
            StartCoroutine(ClimbingFinishedDelay());
            base.FinishInteraction();
        }

        private IEnumerator ClimbingFinishedDelay()
        {
            yield return new WaitForSeconds(0.5f);
            _climbing = false;
        }

        private void Update()
        {
            if (_canClimb && !_climbing)
            {
                PlayerMove.Movedir playerDirRight = ClimbInteractableManager.instance.GetPlayerMovingRight();
                _obsDir = climbRight ? PlayerMove.Movedir.Right : PlayerMove.Movedir.Left;
                //print("player right: " + playerDirRight);
                if (playerDirRight == _obsDir)
                {
                    _climbing = true;
                    //_currentlyInteractable = false;
                    ClimbDown();
                }
            }
        }
    }
}