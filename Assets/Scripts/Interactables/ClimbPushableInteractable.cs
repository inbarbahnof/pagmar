using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace Interactables
{
    public class ClimbPushableInteractable : ClimbObject
    {
        [SerializeField] private Transform leftPlayerClimbPos;
        [SerializeField] private Transform rightPlayerClimbPos;
        [SerializeField] private Transform leftPlayerDownPos;
        [SerializeField] private Transform rightPlayerDownPos;
        [SerializeField] private GameObject leftClimbCol;
        [SerializeField] private GameObject rightClimbCol;
        [SerializeField] private bool playerFromLeft = true;
        private bool _onTop = false;
        private GameObject curCol;
        Vector2 curPlayerPos = Vector2.zero;
        
        public override void Interact()
        {
            ClimbInteractableManager.instance.Climb(this, curPlayerPos, !_onTop, playerFromLeft);
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
            curCol = rightClimbCol;
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
        }

    }
}