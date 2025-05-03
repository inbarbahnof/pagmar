using System;
using UnityEngine;

namespace Dog
{
    public class DogTargetDetector : MonoBehaviour
    {
        [SerializeField] private DogActionManager _actionManager;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Food"))
            {
                _actionManager.FoodIsClose(other);
            }
        }
    }
}
