using System;
using Dog;
using Interactables;
using Unity.VisualScripting;
using UnityEngine;

namespace CheckpointUtils
{
    public class CheckpointOriginator : MonoBehaviour
    {
        [SerializeField] private PlayerMove player;
        [SerializeField] private DogActionManager dog;
        private Vector3 dogRespawnOffset = new Vector3(-1f, -1f, 0f);
        private Obstacle _curObs = null;
    
        private CheckpointInfo _curCheckpointInfo;

        private void Start()
        {
            _curCheckpointInfo = new CheckpointInfo(
                transform.position, 
                player.transform.position, 
                player.transform.position + dogRespawnOffset);
        }

        public IMemento Save(Vector2 checkpointLoc, Vector3 playerRespawnPoint, Obstacle obstacle = null)
        {
            _curObs = obstacle;
            _curCheckpointInfo = new CheckpointInfo(
                checkpointLoc, 
                playerRespawnPoint, 
                playerRespawnPoint + dogRespawnOffset, 
                _curObs);
            return new ConcreteMemento(_curCheckpointInfo);
        }

        public void Restore(IMemento memento)
        {
            if (!(memento is ConcreteMemento))
            {
                throw new Exception("Unknown memento class " + memento.ToString());
            }
            _curCheckpointInfo = memento.GetCheckpointInfo();
            CameraController.instance.FollowPlayer();
            // restore player and dog
            player.ResetToCheckpoint(_curCheckpointInfo.PlayerRespawnLoc);
            dog.ResetToCheckpoint(_curCheckpointInfo.DogLoc);
            if (_curCheckpointInfo.CurObstacle != null)
            {
                _curObs.ResetObstacle();
            }
        }
    }
}
