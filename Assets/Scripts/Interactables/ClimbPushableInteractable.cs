using System.Collections;
using NUnit.Framework.Constraints;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace Interactables
{
    public class ClimbPushableInteractable : ClimbObject
    {
        [SerializeField] private Transform playerPos;
        [SerializeField] private bool climbDown = false;
        [SerializeField] private ClimbPushManager climbPushManager;

        public override void Interact()
        {
            if (twinTrigger is null && twin is not null) twinTrigger = twin.GetColGO();
            ClimbInteractableManager.instance.Climb(this, playerPos.position, _isClimbing, climbRight);
            if (col)
            {
                col.SetActive(false);
                //print("col deactivated at interact");
            }
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