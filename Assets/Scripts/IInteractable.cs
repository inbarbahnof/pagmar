using UnityEngine;

public interface IInteractable
{
    void Interact(Transform player, Transform dog);
    void SetHighlight(bool isHighlighted);
}
