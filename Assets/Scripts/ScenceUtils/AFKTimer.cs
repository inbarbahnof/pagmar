using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core
{
    public class AFKTimer : MonoBehaviour
    {
        [SerializeField] private float timeoutSeconds = 60f;

        private float _timeSinceLastInput = 0f;
        private IDisposable _inputListener;

        private void OnEnable()
        {
            _timeSinceLastInput = 0f;
            _inputListener = InputSystem.onAnyButtonPress.Call(OnInputReceived);
        }

        private void OnDisable()
        {
            _inputListener.Dispose();
        }

        private void Update()
        {
            if (GameManager.instance.AtGameStart) return;
            
            _timeSinceLastInput += Time.unscaledDeltaTime;

            if (_timeSinceLastInput >= timeoutSeconds)
            {
                ResetGame();
            }
        }

        private void OnInputReceived(InputControl control)
        {
            _timeSinceLastInput = 0f;
        }

        private void ResetGame()
        {
            _inputListener.Dispose();
            GameManager.instance.RestartGame();
        }
    }
}
