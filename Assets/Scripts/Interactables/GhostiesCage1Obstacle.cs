using System;
using Ghosts;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace Interactables
{
    public class GhostiesCage1Obstacle : Obstacle
    {
        [SerializeField] private GhostieDie[] _ghosties;
        [SerializeField] private GameObject _TOIs;
        [SerializeField] private Cage _cage;

        private void Start()
        {
            _cage.OnGhostEnterCage += GhostieInCage;
        }

        public override void ResetObstacle()
        {
            foreach (var ghostie in _ghosties)
            {
                ghostie.Live();
            }
        }

        private void GhostieInCage()
        {
            _TOIs.SetActive(false);
        }
    }
}
