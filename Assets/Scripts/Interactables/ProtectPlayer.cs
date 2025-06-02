using System;
using UnityEngine;

public class ProtectPlayer : MonoBehaviour
{
    [SerializeField] private bool _isProtected = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStealthManager>().SetProtected(_isProtected);
        }
    }
}
