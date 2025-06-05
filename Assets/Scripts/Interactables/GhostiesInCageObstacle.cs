using System;
using System.Collections;
using DG.Tweening;
using Ghosts;
using UnityEngine;

namespace Interactables
{
    public class GhostiesInCageObstacle : Obstacle
    {
        [SerializeField] private Cage _cage;
        [SerializeField] private Transform _cageDropPos;
        [SerializeField] private float _dropDuration = 1.3f;
        [SerializeField] private GameObject _deathZoneToDestroy;
        [SerializeField] private GhostieDie[] _ghosties;

        private void Start()
        {
            _cage.OnGhostEnterCage += HandleGhostEnterCage;
        }

        private void HandleGhostEnterCage()
        {
            if (_cage.GhostiesInCage >= 2)
            {
                _cage.transform.DOMove(_cageDropPos.position, _dropDuration)
                    .SetEase(Ease.OutBounce)
                    .OnComplete(AfterCageDrop);
            }
        }

        private void AfterCageDrop()
        {
            _deathZoneToDestroy.gameObject.SetActive(false);
            NavMeshManager.instance.ReBake();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
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
            CameraController.instance.ZoomIn();
        }
            
    }
}