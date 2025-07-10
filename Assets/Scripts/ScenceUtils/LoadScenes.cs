using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Unity.Cinemachine;

public class LoadScenes : MonoBehaviour
{
    [Header("Scenes To Load")]
    [SerializeField] private SceneField _sceneToLoad;

    [Header("Player Movement")]
    [SerializeField] private SmoothMover _player;
    [SerializeField] private SmoothMover _dog;

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
        
        // Start loading the new scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneToLoad);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}