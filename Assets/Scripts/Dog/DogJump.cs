using System;
using System.Collections;
using UnityEngine;

namespace Dog
{
    public class DogJump : MonoBehaviour
    {
        [SerializeField] private GameObject _secondSideJump;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Dog"))
            {
                other.GetComponent<DogAnimationManager>().DogJumping(true);
                StartCoroutine(EnableSecondSide());
            }
        }

        private IEnumerator EnableSecondSide()
        {
            yield return new WaitForSeconds(1.8f);
            _secondSideJump.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
