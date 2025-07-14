using System;
using System.Collections;
using Audio.FMOD;
using DG.Tweening;
using Ghosts;
using UnityEngine;

namespace Interactables
{
    public class GhostiesInCage2Obstacle : Obstacle
    {
        [SerializeField] private Cage _cage;
        [SerializeField] private Transform _cageDropPos;
        [SerializeField] private float _dropDuration = 1.3f;
        [SerializeField] private GameObject _deathZoneToDestroy;
        [SerializeField] private GhostieDie[] _ghosties;
        [SerializeField] private Animator _cageAnimator;


        private void Start()
        {
            _cage.OnGhostEnterCage += HandleGhostEnterCage;
        }

        public void ShowCage()
        {
            CameraController.instance.ExtraZoomOut(true);
        }

        private void HandleGhostEnterCage()
        {
            if (_cage.GhostiesInCage >= 1)
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.CageSnap);
                CameraController.instance.ExtraZoomOut(false);
                
                _cageAnimator.SetTrigger("fall");
                
                _cage.transform.DOMove(_cageDropPos.position, _dropDuration)
                    .SetEase(Ease.InCubic)
                    .OnComplete(AfterCageDrop);
            }
        }

        private void AfterCageDrop()
        {
            _deathZoneToDestroy.gameObject.SetActive(false);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.CageFall);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // print("zoom out " + _isZoomOut);
            if (other.CompareTag("Player"))
            {
                CameraController.instance.ZoomOut();
            }
        }

        public override void ResetObstacle()
        {
            foreach (var ghostie in _ghosties)
            {
                ghostie.Live();
            }
            
            _cage.ResetCage();
        }
            
    }
}