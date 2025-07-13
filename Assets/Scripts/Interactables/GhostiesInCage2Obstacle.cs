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

        private bool _isZoomOut;

        private void Start()
        {
            _cage.OnGhostEnterCage += HandleGhostEnterCage;
        }

        private void HandleGhostEnterCage()
        {
            if (_cage.GhostiesInCage >= 1)
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.CageSnap);
                CameraController.instance.ExtraZoomOut();
                _cage.transform.DOMove(_cageDropPos.position, _dropDuration)
                    .SetEase(Ease.InCubic)
                    .OnComplete(AfterCageDrop);
            }
        }

        private void AfterCageDrop()
        {
            _deathZoneToDestroy.gameObject.SetActive(false);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.CageFall);
            // NavMeshManager.instance.ReBake();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // print("zoom out " + _isZoomOut);
            if (other.CompareTag("Player"))
            {
                if (_isZoomOut)
                {
                    _isZoomOut = false;
                    CameraController.instance.ZoomIn();
                }
                else
                {
                    _isZoomOut = true;
                    CameraController.instance.ZoomOut();
                }
                
            }
        }

        public override void ResetObstacle()
        {
            foreach (var ghostie in _ghosties)
            {
                ghostie.Live();
            }
            
            _cage.ResetCage();
            CameraController.instance.ZoomIn();
            _isZoomOut = false;
        }
            
    }
}