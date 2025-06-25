using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent triggerAction;
    [SerializeField] private bool turnOffAfterTrigger;
    [SerializeField] private bool isPlayerEmmiting = true;
    private bool _triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isPlayerEmmiting && !_triggered)
        {
            triggerAction?.Invoke();
            _triggered = true;
        }

        if (other.CompareTag("Dog") && !isPlayerEmmiting && !_triggered)
        {
            triggerAction?.Invoke();
            _triggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isPlayerEmmiting)
        {
            _triggered = false;
            if (turnOffAfterTrigger) gameObject.GetComponent<Collider2D>().enabled = false;
        }
        else if (other.CompareTag("Dog") && !isPlayerEmmiting)
        {
            
            _triggered = false;
            if (turnOffAfterTrigger) gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
