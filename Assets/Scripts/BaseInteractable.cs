using System;
using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject highlightEffect;

    private bool _isInteracting = false;
    
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

    // player pressed button
    public virtual void Interact(Transform player)
    {
        _isInteracting = true;
    }

    // player let go of button
    public virtual void StopInteractPress()
    {
        // if done need to finish interaction
        // this may be just picking up an obj and then not done until press again to drop
    }

    // task complete / not performing task anymore
    protected void FinishInteraction()
    {
        _isInteracting = false;
        InteractableManager.instance.OnFinishInteraction();
    }
}