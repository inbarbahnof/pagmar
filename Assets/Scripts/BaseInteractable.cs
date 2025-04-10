using System;
using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject highlightEffect;
    protected bool readyToInteract = false;

    public void SetHighlight(bool isHighlighted)
    {
        highlightEffect.SetActive(isHighlighted);
        readyToInteract = isHighlighted;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SetHighlight(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SetHighlight(false);
        }
    }

    public abstract void Interact();
}