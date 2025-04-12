using UnityEngine;

public interface IInteractable
{
    void Interact(Transform player);
    void SetHighlight(bool isHighlighted);
}
