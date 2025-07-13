using UnityEngine;

public class MultiSpriteFadeTrigger : MonoBehaviour
{
    [SerializeField] private SpriteFade[] objects;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        foreach (var obj in objects)
        {
            obj.FadeOutOverTime(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        foreach (var obj in objects)
        {
            obj.FadeOutOverTime(false);
        }
    }
}