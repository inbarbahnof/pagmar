using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Unity.Cinemachine;

public class LoadScenes : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private SmoothMover _player;
    [SerializeField] private SmoothMover _dog;

    [Header("Start Scene")]
    [SerializeField] private Transform _playerStartLevelPos;
    [SerializeField] private Transform _dogStartLevelPos;

    private void Start()
    {
        if (_playerStartLevelPos != null) _player.MoveTo(_playerStartLevelPos.position, 1f);
        if (_dogStartLevelPos != null) _dog.MoveTo(_dogStartLevelPos.position, 1f);
    }

    public void LoadScene()
    {
        StartCoroutine(LoadSceneWithCameraMovement());
    }

    private IEnumerator LoadSceneWithCameraMovement()
    {
        _player.MoveTo(_player.transform.position + Vector3.right*6, 2f);
        _dog.MoveTo(_dog.transform.position + Vector3.right*6, 2f);

        yield return new WaitForSeconds(1f);
        
        CameraController.instance.ExtraZoomIn();
        yield return new WaitForSeconds(1f);
        GameManager.instance.LevelEnd();
    }
}