using Audio.FMOD;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
        [SerializeField] private GameObject usCredits;
        [SerializeField] private GameObject themCredits;
        
        [SerializeField] private float nextCreditsWaitTime = 1f;
        [SerializeField] private float creditsFadeOutWaitTime = 4f;
        [SerializeField] private float outAnimTime = 0.5f;
        [SerializeField] private float endWaitTime = 3f;

        private Animator _usAnimator;
        private Animator _themAnimator;
        private Animator _curAnimator;

        private void Start()
        {
                _usAnimator = usCredits.GetComponent<Animator>();
                _themAnimator = themCredits.GetComponent<Animator>();

        if (AudioManager.Instance.musicInstance.getPlaybackState(out var playbackState) == FMOD.RESULT.OK)
        {
            if (playbackState == FMOD.Studio.PLAYBACK_STATE.PLAYING)
                AudioManager.Instance.StopMusic();
        }


    }

        public void SetCurAnimator(Animator animator)
        {
                _curAnimator = animator;
        }

        public void StartFadeOut()
        {
                StartCoroutine(FadeOut());
        }

        private IEnumerator FadeOut()
        {
                yield return new WaitForSecondsRealtime(creditsFadeOutWaitTime);
                _curAnimator.SetTrigger("out");
                yield return new WaitForSeconds(outAnimTime);
                FadeOutComplete();
        }

        private void FadeOutComplete()
        {
                StartCoroutine(StartNextCredits());
        }
        
        private IEnumerator StartNextCredits()
        {
                yield return new WaitForSecondsRealtime(nextCreditsWaitTime);
                if (_curAnimator == _usAnimator)
                {
                        usCredits.SetActive(false);
                        themCredits.SetActive(true);
                }
                else if (_curAnimator == _themAnimator)
                {
                        yield return new WaitForSecondsRealtime(endWaitTime);
                        themCredits.SetActive(false);
                        GameManager.instance.RestartGame();
                }
        }
        
        
}