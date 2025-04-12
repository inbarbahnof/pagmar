using UnityEngine;

public class PickUpParentManager : MonoBehaviour
{
    [SerializeField] private Transform _pickUpParent;

    public Transform GetPickUpParent()
    {
        return _pickUpParent;
    }
}
