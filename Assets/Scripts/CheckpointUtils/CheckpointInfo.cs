using Interactables;
using UnityEngine;

namespace CheckpointUtils
{
    public struct CheckpointInfo
    {
        public Vector2 PlayerLoc;
        public Vector2 DogLoc;
        public Obstacle CurObstacle;

        public CheckpointInfo(Vector2 playerLoc, Vector2 dogLoc, Obstacle curObstacle = null)
        {
            PlayerLoc = playerLoc;
            DogLoc = dogLoc;
            CurObstacle = curObstacle;
        }

    }
}