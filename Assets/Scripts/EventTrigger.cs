using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent triggerAction;
    [SerializeField] private bool turnOffAfterTrigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) triggerAction?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && turnOffAfterTrigger)
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
