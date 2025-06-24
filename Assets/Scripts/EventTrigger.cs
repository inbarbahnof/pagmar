using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent triggerAction;
    [SerializeField] private bool turnOffAfterTrigger;
    [SerializeField] private bool isPlayerEmmiting = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isPlayerEmmiting) triggerAction?.Invoke();
        if (other.CompareTag("Dog") && !isPlayerEmmiting) triggerAction?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && turnOffAfterTrigger && isPlayerEmmiting)
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
        else if (other.CompareTag("Dog") && turnOffAfterTrigger && !isPlayerEmmiting)
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
