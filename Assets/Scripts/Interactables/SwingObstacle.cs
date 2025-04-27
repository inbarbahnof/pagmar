using UnityEngine;

namespace Interactables
{
    public class SwingObstacle : Obstacle
    {
        [SerializeField] private SwingInteractableManager swingManager;
        private GameObject deathZone;

        void Start()
        {
            swingManager.SetSwingEndAction(StartSwinging, StopSwinging);
            deathZone = GetComponentInChildren<PlayerDeathZone>().gameObject;
        }
        
        public void StartSwinging()
        {
            deathZone.SetActive(false);
        }

        public void StopSwinging()
        {
            deathZone.SetActive(true);
        }

        public override void ResetObstacle()
        {
            swingManager.ResetToCheckpoint();
        }
    }
}
