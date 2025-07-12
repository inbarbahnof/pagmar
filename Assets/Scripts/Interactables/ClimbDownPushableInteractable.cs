using System;
using System.Collections;
using UnityEngine;

namespace Interactables
{
    public class ClimbDownPushableInteractable : ClimbPushableInteractable
    {
        private bool _canClimb = false;
        private bool _climbing = false;
        
        public override void Interact()
        {
            return;
        }

        public override void SetHighlight(bool isHighlighted)
        {
            _canClimb = isHighlighted;
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
                bool playerDirRight = ClimbInteractableManager.instance.GetPlayerMovingRight();
                //print("player right: " + playerDirRight);
                if (playerDirRight == climbRight)
                {
                    _climbing = true;
                    ClimbDown();
                }
            }
        }
    }
}