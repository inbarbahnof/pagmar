using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class InteractableManager : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private Transform dog;

        public static InteractableManager instance;

        private List<BaseInteractable> curInteractables = new List<BaseInteractable>();
        private BaseInteractable curInteractableObj;
        private bool interacting = false;
        private PlayerStateManager playerStateManager;

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else Debug.LogError("TOO MANY INTERACTABLE MANAGERS!");

            playerStateManager = player.GetComponent<PlayerStateManager>();
        }

        public void AddInteractableObj(BaseInteractable interactable)
        {
            curInteractables.Add(interactable);
            //print("inter added: " + interactable.name);
        }

        public void RemoveInteractable(BaseInteractable interactable)
        {
            curInteractables.Remove(interactable);
            //print("inter removed: " + interactable + " at dist: " + Vector2.Distance(player.position, interactable.transform.position));
            if (curInteractableObj == interactable)
            {
                curInteractableObj.SetHighlight(false);
                curInteractableObj = null;
            }
        }

        private void Update()
        {
            if (!interacting)
            {
                RemoveDistantInteractables();
                UpdateClosestInteractable();
            }
        }

        private void RemoveDistantInteractables()
        {
            for (int i = curInteractables.Count - 1; i >= 0; i--)
            {
                BaseInteractable interactable = curInteractables[i];
                if (interactable != null)
                {
                    float dist = Vector2.Distance(player.position, interactable.GetCurPos());
                    float interactionRange = interactable.InteractionRange;
                    if (dist > interactionRange)
                    {
                        // print("removing " + interactable.name + " at distance " + dist);
                        RemoveInteractable(interactable);
                    }
                }
            }
        }

        private void UpdateClosestInteractable()
        {
            if (curInteractables.Count == 0) return;

            BaseInteractable closest = null;
            float minDist = float.MaxValue;
            foreach (BaseInteractable interactable in curInteractables)
            {
                if (interactable == null)
                {
                    curInteractables.Remove(interactable);
                    continue;
                }

                float curDist = Vector2.Distance(player.position, interactable.transform.position);
                if (curDist < minDist && interactable.CanInteract)
                {
                    minDist = curDist;
                    closest = interactable;
                }
            }
            //print("closest: " + closest + Vector2.Distance(player.position, closest.transform.position));
    
            if (closest == null) return;
            if (curInteractableObj != closest)
            {
                if (curInteractableObj != null)
                {
                    curInteractableObj.SetHighlight(false);
                }

                curInteractableObj = closest;
                curInteractableObj.SetHighlight(true);
            }
        }

        // player pressed interact button
        public void OnInteract()
        {
            if (curInteractableObj != null)
            {
                curInteractableObj.SetHighlight(false);
                curInteractableObj.Interact();
                interacting = true;

                playerStateManager.UpdateCurInteraction(curInteractableObj);
            }
        }

        // player stopped pressing button
        public void OnStopInteract()
        {
            if (curInteractableObj != null)
            {
                curInteractableObj.StopInteractPress();
            }
        }
        
        public void FinishPickupInteraction(PickUpInteractable pickup, Vector2 worldTarget, Transform parent)
        {
            StartCoroutine(FinishPickupCoroutine(pickup, worldTarget, parent));
        }

        private IEnumerator FinishPickupCoroutine(PickUpInteractable pickup, Vector2 worldTarget, Transform parent)
        {
            yield return new WaitForSeconds(0.2f);

            pickup.transform.SetParent(parent);
            if (worldTarget != Vector2.zero)
                pickup.transform.position = worldTarget;

            Audio.FMOD.AudioManager.Instance.PlayOneShot(Audio.FMOD.FMODEvents.Instance.PlayerPickUp);
            pickup.FinishInteraction();
        }

        public void OnFinishInteraction(BaseInteractable interactableTwin = null)
        {
            interacting = false;

            if (curInteractableObj)
            {
                if (interactableTwin is null)
                {
                    curInteractableObj.SetHighlight(true);
                }
                else
                {
                    AddInteractableObj(interactableTwin);
                    RemoveInteractable(curInteractableObj);
                }
            }

            playerStateManager.OnFinishedInteraction(curInteractableObj);
        }

    }
}