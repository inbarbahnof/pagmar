using System;
using UnityEngine;

namespace Interactables
{
    public class ThrowStickOnTreeDetector : MonoBehaviour
    {
        [SerializeField] private ThrowStickOnFoodObstacle _throwStickOnFoodObstacle;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Stick"))
            {
                if (other.GetComponent<ThrowablePickUpInteractable>().IsThrowing)
                    _throwStickOnFoodObstacle.DropStick();
            }
        }
    }
}
