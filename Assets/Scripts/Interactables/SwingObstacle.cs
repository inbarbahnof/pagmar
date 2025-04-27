using UnityEngine;
using UnityEngine.Serialization;

namespace Interactables
{
    public class SwingObstacle : Obstacle
    {
        [SerializeField] private SwingInteractableManager swingManager;
        [SerializeField] private SwingInteractable interactable;
        [SerializeField] private GameObject deathZone;

        void Start()
        {
            swingManager.SetSwingEndAction(StartSwinging, StopSwinging);
        }
        
        public void StartSwinging()
        {
            if (deathZone is not null) deathZone.SetActive(false);
        }

        public void StopSwinging()
        {
            if (deathZone is not null) deathZone.SetActive(true);
        }

        public override void ResetObstacle()
        {
            swingManager.ResetToCheckpoint(interactable);
        }
    }
}
