using System;
using Interactables;
using UnityEngine;

namespace CheckpointUtils
{
    public class CheckpointSavePoint : MonoBehaviour
    {
        [SerializeField] private Obstacle obstacle = null;
        [SerializeField] private GameObject playerRespawnPoint;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.checkpointManagerInstance.Backup(
                    transform.position, 
                    playerRespawnPoint.transform.position, 
                    obstacle);
            }
        }
    }
}