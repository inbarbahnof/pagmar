using System;
using UnityEngine;

public class CreditAnimController : MonoBehaviour
{
        [SerializeField] private CreditsManager creditsManager;

        private void Start()
        {
                creditsManager.SetCurAnimator(this.GetComponent<Animator>());
        }

        public void EndFadeIn()
        {
                creditsManager.StartFadeOut();
        }
}