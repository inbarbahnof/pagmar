using System.Collections;
using NUnit.Framework.Constraints;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace Interactables
{
    public class ClimbPushableInteractable : ClimbObject
    {
        [SerializeField] private Transform playerPos;
        [SerializeField] private bool finishInteractAtEnd = false;

        public override void Interact()
        {
            if (twinTrigger is null && twin is not null) twinTrigger = twin.GetColGO();
            ClimbInteractableManager.instance.Climb(this, playerPos.position, _isClimbing, climbRight);
            col.SetActive(false);
        }

        public override void FinishInteraction()
        {
            if (finishInteractAtEnd) ClimbInteractableManager.instance.StopInteraction();
            base.FinishInteraction();
        }


        /*[SerializeField] private Transform leftPlayerClimbPos;
        [SerializeField] private Transform rightPlayerClimbPos;
        [SerializeField] private Transform leftPlayerDownPos;
        [SerializeField] private Transform rightPlayerDownPos;
        [SerializeField] private GameObject leftClimbCol;
        [SerializeField] private GameObject rightClimbCol;
        [SerializeField] private bool playerFromLeft = true;
        private bool _onTop = false;
        private GameObject curCol;
        Vector2 curPlayerPos = Vector2.zero;
        private bool triggered = false;

        public override void Interact()
        {
            ClimbInteractableManager.instance.Climb(this, curPlayerPos, !_onTop, playerFromLeft);
            print("climbing: " + this.name + " from pos: " + curPlayerPos + " onTop: " + !_onTop + " fromLeft: " + playerFromLeft);
            //curCol.SetActive(false);
            _onTop = !_onTop;
        }

        public override void StopInteractPress()
        {
            StartCoroutine(WaitToStopInteract());
        }

        private IEnumerator WaitToStopInteract()
        {
            float time;
            if (!_onTop) time = 2.1f;
            else time = 0.8f;

            yield return new WaitForSeconds(time);
            base.FinishInteraction();
        }

        public void EnteredLeftTrigger()
        {
            curCol = leftClimbCol;
            if (!_onTop)
            {
                InteractableManager.instance.AddInteractableObj(this);
                playerFromLeft = true;
                curPlayerPos = leftPlayerClimbPos.position;
            }
            else
            {
                curPlayerPos = leftPlayerDownPos.position;
                Interact();
            }
        }

        public void EnteredRightTrigger()
        {
            if (!_onTop)
            {
                InteractableManager.instance.AddInteractableObj(this);
                playerFromLeft = false;
                curPlayerPos = rightPlayerClimbPos.position;
            }
            else
            {
                curPlayerPos = rightPlayerDownPos.position;
                Interact();
            }
            curCol = rightClimbCol;
        }
        */

    }
}