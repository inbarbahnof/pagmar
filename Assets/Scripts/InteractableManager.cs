using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InteractableManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    
    public static InteractableManager instance;
    
    private List<BaseInteractable> curInteractables;
    private BaseInteractable curInteractableObj;
    private bool interacting = false;
    
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Debug.LogError("TOO MANY INTERACTABLE MANAGERS!");
    }

    public void AddInteractableObj(BaseInteractable interactable)
    {
        curInteractables.Add(interactable);
    }

    public void RemoveInteractable(BaseInteractable interactable)
    {
        curInteractables.Remove(interactable);
    }

    private void Update()
    {
        if (!interacting)
        {
            UpdateClosestInteractable();
        }
    }

    private void UpdateClosestInteractable()
    {
        if (curInteractables.Count == 0) return;
        BaseInteractable closest = (curInteractableObj == null) ? curInteractables[0] : curInteractableObj;
        float dist = Vector2.Distance(player.position, closest.transform.position);
        foreach(BaseInteractable interactable in curInteractables)
        {
            float curDist = Vector2.Distance(player.position, interactable.transform.position);
            if (curDist < dist)
            {
                dist = curDist;
                closest = interactable;
            }
        }

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
        curInteractableObj.Interact();
        interacting = true;
    }

    // player stopped pressing button
    public void OnStopInteract()
    {
        curInteractableObj.StopInteract();
    }

    public void OnFinishInteraction()
    {
        interacting = false;
    }
    
}
