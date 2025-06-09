using System;
using Dog;
using UnityEngine;

public class StealthObject : MonoBehaviour
{
    private PlayerStealthManager _curStealthManager;
    private DogActionManager _curDogActionManager;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _curStealthManager = other.GetComponent<PlayerStealthManager>();
           _curStealthManager.SetStealthMode(true);
           //print("player in stealth mode");
        }
        else if (other.CompareTag("Dog"))
        {
            _curDogActionManager = other.GetComponent<DogActionManager>();
            _curDogActionManager.HandleDogProtectionChanged(true);
            //print("dog in stealth mode");
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _curStealthManager.SetStealthMode(false);
            _curStealthManager = null;
            //print("player exit stealth mode");
        }
        else if (other.CompareTag("Dog"))
        {
            _curDogActionManager.HandleDogProtectionChanged(false);
            _curDogActionManager = null;
            //print("dog exit stealth mode");
        }
    }
}
