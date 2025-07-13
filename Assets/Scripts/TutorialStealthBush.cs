using System;
using UnityEngine;

public class TutorialStealthBush : MonoBehaviour
{
    [SerializeField] private DogMeetManager _manager;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _manager.PlayerHiding(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _manager.PlayerHiding(false);
    }
}