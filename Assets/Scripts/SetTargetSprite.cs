using System;
using UnityEngine;

public class SetTargetSprite : MonoBehaviour
{
    [SerializeField] private GameObject _regularTarget;
    [SerializeField] private GameObject _airTarget;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AirThrow"))
        {
            _regularTarget.SetActive(false);
            _airTarget.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("AirThrow"))
        {
            _regularTarget.SetActive(true);
            _airTarget.SetActive(false);
        }
    }
}
