using System;
using Interactables;
using Unity.VisualScripting;
using UnityEngine;

namespace CheckpointUtils
{
    public class CheckpointOriginator : MonoBehaviour
    {
        [SerializeField] private PlayerMove player;
        [SerializeField] private GameObject dog;
        private Obstacle _curObs = null;
    
        private CheckpointInfo _curCheckpointInfo;
    
        private void Start()
        {
            _curCheckpointInfo = new CheckpointInfo(player.transform.position, dog.transform.position);
        }

        public IMemento Save(Obstacle obstacle = null)
        {
            _curObs = obstacle;
            _curCheckpointInfo = new CheckpointInfo(player.transform.position, dog.transform.position, _curObs);
            return new ConcreteMemento(_curCheckpointInfo);
        }

        public void Restore(IMemento memento)
        {
            if (!(memento is ConcreteMemento))
            {
                throw new Exception("Unknown memento class " + memento.ToString());
            }
            _curCheckpointInfo = memento.GetCheckpointInfo();
            // restore player and dog
            player.GetComponent<PlayerMove>().ResetToCheckpoint(_curCheckpointInfo.PlayerLoc);
            // TODO reset dog, finish reset player
        }
    }
}
