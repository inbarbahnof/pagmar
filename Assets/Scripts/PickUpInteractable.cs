using System.Collections;
using UnityEngine;

public class PickUpInteractable : BaseInteractable
{
    private bool isPickedUp = false;
    private Transform originalParent;
    
    public override void Interact(Transform player)
    {
        base.Interact(player);
        
        if (isPickedUp) DropObject();

        else PickUpObject(player);
    }

    private void PickUpObject(Transform player)
    {
        originalParent = transform.parent;
        
        Transform pickUpParent = player.GetComponent<PickUpParentManager>().GetPickUpParent();
        transform.SetParent(pickUpParent);

        transform.localPosition = Vector3.zero;

        isPickedUp = true;
    }
    
    private void DropObject()
    {
        isPickedUp = false;
        transform.SetParent(originalParent);

        StartCoroutine(FinishAction());
    }

    private IEnumerator FinishAction()
    {
        yield return new WaitForSeconds(0.1f);
        FinishInteraction();
    }
}
