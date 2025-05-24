using System;
using UnityEngine;

public class StealthObject : MonoBehaviour
{
    private PlayerStealthManager _curStealthManager;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _curStealthManager = other.GetComponent<PlayerStealthManager>();
           _curStealthManager.SetStealthMode(true);
           print("player in stealth mode");
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _curStealthManager.SetStealthMode(false);
            _curStealthManager = null;
            print("player exit stealth mode");
        }
    }
}
