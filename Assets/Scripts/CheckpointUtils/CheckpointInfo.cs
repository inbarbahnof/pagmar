using Interactables;
using UnityEngine;

namespace CheckpointUtils
{
    public struct CheckpointInfo
    {
        public Vector2 CheckpointLoc;
        public Vector2 PlayerRespawnLoc;
        public Vector2 DogLoc;
        public Obstacle CurObstacle;

        public CheckpointInfo(
            Vector2 checkpointLoc, 
            Vector2 playerRespawnLoc, 
            Vector2 dogLoc, 
            Obstacle curObstacle = null)
        {
            CheckpointLoc = checkpointLoc;
            PlayerRespawnLoc = playerRespawnLoc;
            DogLoc = dogLoc;
            CurObstacle = curObstacle;
        }

    }
}