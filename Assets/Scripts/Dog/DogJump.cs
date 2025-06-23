using System;
using System.Collections;
using UnityEngine;

namespace Dog
{
    public class DogJump : MonoBehaviour
    {
        [SerializeField] private DogJumpManager manager;
        [SerializeField] private bool isLeftTrigger;
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Dog"))
            {
                manager.OnTriggerEntered(other.gameObject, isLeftTrigger);
            }
        }
    }
}