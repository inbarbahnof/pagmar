using Interactabels;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Interactables
{
    public class SwingObstacle : Obstacle
    {
        [SerializeField] private SwingInteractableManager swingManager;
        private bool _setupComplete;
        private GameObject deathZone;

        void Start()
        {
            swingManager.SetSwingEndAction(StartSwinging, StopSwinging);
            deathZone = GetComponentInChildren<PlayerDeathZone>().gameObject;
        }

        public void ReachedTarget()
        {
            _setupComplete = true;
        }

        public void StartSwinging()
        {
            deathZone.SetActive(false);
        }

        public void StopSwinging()
        {
            deathZone.SetActive(true);
        }

    }
}
