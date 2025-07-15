using UnityEngine;

namespace Interactables
{
    public class ClimbPushableInteractable : ClimbObject
    {
        [SerializeField] protected Transform playerPos;
        [SerializeField] private bool climbDown = false;
        [SerializeField] private ClimbPushManager climbPushManager;
        protected bool _interacting;

        public override void Interact()
        {
            bool playerDirRight = ClimbInteractableManager.instance.GetPlayerMovingRight();
            if (playerDirRight != climbRight || !interTrigger.enabled || !_currentlyInteractable)
            {
                InteractableManager.instance.OnFinishInteraction();
                return;
            }
            _currentlyInteractable = false;
            _interacting = true;
            
            if (twinTrigger is null && twin is not null) twinTrigger = twin.GetColGO();
            ClimbInteractableManager.instance.Climb(this, playerPos.position, _isClimbing, climbRight);
            if (col)
            {
                col.SetActive(false);
                //print("col deactivated at interact");
            }
            if (textPrompt) textPrompt.StopShowText();
        }
        
        public override void StopInteractPress()
        {
            base.StopInteractPress();
            if (!_interacting)
            {
                InteractableManager.instance.OnFinishInteraction();
                return;
            }
            _interacting = false;
        }

        public override void FinishInteraction()
        {
            if (climbDown)
            {
                ClimbInteractableManager.instance.StopInteraction();
                climbPushManager.PlayerOnOffLog(false);
                //print("player off log");
            }
            else climbPushManager.PlayerOnOffLog(true);
            //print("player on log");
            base.FinishInteraction();
        }

    }
}