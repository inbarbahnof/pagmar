using UnityEngine;

namespace Interactabels
{
    public interface IInteractable
    {
        void Interact();
        void SetHighlight(bool isHighlighted);
    }
}
