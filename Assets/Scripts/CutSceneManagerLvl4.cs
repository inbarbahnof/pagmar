using System;
using UnityEngine;

public class CutSceneManagerLvl4 : MonoBehaviour
{
    [SerializeField] private Transform _playerPos;
    [SerializeField] private Transform _dogPos;
    [SerializeField] private SmoothMover _player;
    [SerializeField] private SmoothMover _dog;
    
    private StartCutsceneManager _cutsceneManager;
    private bool _waiting;

    private void Start()
    {
        _cutsceneManager = GetComponent<StartCutsceneManager>();
    }
    
    public void MoveToPositions()
    {
        _cutsceneManager.FreezePositions();
        _player.MoveTo(_playerPos.position, 5f);
        _dog.MoveTo(_dogPos.position, 5f);
        _waiting = true;
    }

    private void Update()
    {
        if (!_waiting) return;

        if (Vector3.Distance(_player.transform.position, _playerPos.position) < 0.3
            && Vector3.Distance(_dog.transform.position, _dogPos.position) < 0.8)
        {
            _cutsceneManager.ShowSequence();
            _waiting = false;
        }
    }
}
