using System;
using UnityEngine;

public class StealthObject : MonoBehaviour
{
    [SerializeField] private Transform _stealthPos;
    private PlayerStealthManager _curStealthManager;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _curStealthManager = other.GetComponent<PlayerStealthManager>();
           _curStealthManager.SetStealthMode(true);
           print("in stealth mode");
           other.GetComponent<SmoothMover>().MoveTo(_stealthPos.position);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _curStealthManager.SetStealthMode(false);
            _curStealthManager = null;
            print("exit stealth mode");
        }
    }
}
