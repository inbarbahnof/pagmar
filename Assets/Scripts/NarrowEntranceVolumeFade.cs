using UnityEngine;

public class NarrowEntranceVolumeFade : MonoBehaviour
{
    [SerializeField] private GameObject _faderGameObject;
    
    private IObjectFader _fader;

    private void Start()
    {
        if (_faderGameObject != null) _fader = _faderGameObject.GetComponent<IObjectFader>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _fader?.FadeOutOverTime(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _fader?.FadeOutOverTime();
        }
    }
}