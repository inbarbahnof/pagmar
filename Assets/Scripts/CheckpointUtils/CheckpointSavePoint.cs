using System;
using Interactables;
using UnityEngine;

namespace CheckpointUtils
{
    public class CheckpointSavePoint : MonoBehaviour
    {
        [SerializeField] private Obstacle obstacle = null;
        [SerializeField] private GameObject playerRespawnPoint;
        [SerializeField] private GameObject dogRespawnPoint;
        private Vector3 defaultDogOffset = new Vector3(-1, 0, 0);
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.checkpointManagerInstance.Backup(
                    transform.position,
                    playerRespawnPoint.transform.position,
                    (dogRespawnPoint is null)
                        ? playerRespawnPoint.transform.position + defaultDogOffset
                        : dogRespawnPoint.transform.position,
                    obstacle);
            }
        }
    }
}