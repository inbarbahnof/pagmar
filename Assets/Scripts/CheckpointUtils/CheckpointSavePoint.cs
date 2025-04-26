using System;
using Interactables;
using UnityEngine;

namespace CheckpointUtils
{
    public class CheckpointSavePoint : MonoBehaviour
    {
        [SerializeField] private Obstacle obstacle = null;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.checkpointManagerInstance.Backup(obstacle);
            }
        }
    }
}