using System;
using System.Collections;
using UnityEngine;

public class StartScreenManager : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private StartCutsceneManager _cutsceneManager;
    [SerializeField] private GameObject _particals;

    private bool _hasStarted;

    public void OnPressStart()
    {
        if (_hasStarted) return;

        _hasStarted = true;
        _animator.SetTrigger("out");
        StartCoroutine(WaitToStartGame());
    }
    
    private IEnumerator WaitToStartGame()
    {
        _particals.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        _cutsceneManager.ShowSequence();
    }
}
