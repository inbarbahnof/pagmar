using System;
using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject highlightEffect;
    
    public void SetHighlight(bool isHighlighted)
    {
        highlightEffect.SetActive(isHighlighted);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InteractableManager.instance.AddInteractableObj(this);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InteractableManager.instance.RemoveInteractable(this);
        }
    }

    // player pressed button
    public virtual void Interact()
    {
        FinishInteraction();
    }

    // player let go of button
    public void StopInteract()
    {
        // if done need to finish interaction
        // this may be just picking up an obj and then not done until pres again to drop
    }

    // task complete / not performing task anymore
    private void FinishInteraction()
    {
        InteractableManager.instance.OnFinishInteraction();
    }

    
}