using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerNarrowEntrance : MonoBehaviour
{
    [SerializeField] private GameObject _faderGameObject;
    
    private PlayerStateManager _stateManager;
    private IObjectFader _fader;

    private void Start()
    {
        _fader = _faderGameObject.GetComponent<IObjectFader>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_stateManager == null)
                _stateManager = other.GetComponent<PlayerStateManager>();
            _stateManager.OnNarrowPass(true);
            _fader.FadeOutOverTime(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_stateManager == null)
                _stateManager = other.GetComponent<PlayerStateManager>();
            _stateManager.OnNarrowPass(false);
            _fader.FadeOutOverTime();
        }
    }
}
