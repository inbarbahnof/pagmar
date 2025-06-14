using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent triggerAction;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) triggerAction?.Invoke();
    }
}
