using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InteractableManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    
    public static InteractableManager instance;
    
    private List<BaseInteractable> curInteractables = new List<BaseInteractable>();
    private BaseInteractable curInteractableObj;
    private bool interacting = false;
    private float interactionRange = 1.5f;
    
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
        print("inter added: " + interactable.name);
    }

    public void RemoveInteractable(BaseInteractable interactable)
    {
        curInteractables.Remove(interactable);
        print("inter removed: " + interactable + " at dist: " + Vector2.Distance(player.position, interactable.transform.position));
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
            float dist = Vector2.Distance(player.position, interactable.GetCurPos());
            if (dist > interactionRange)
            {
                RemoveInteractable(interactable);
            }
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
        if (curInteractableObj != null)
        {
            curInteractableObj.Interact(player);
            curInteractableObj.SetHighlight(false);
            interacting = true;
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

    public void OnFinishInteraction()
    {
        interacting = false;
        
        if (curInteractableObj != null)
        {
            curInteractableObj.SetHighlight(true);
        }
    } 
    
}
