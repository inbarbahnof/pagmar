using System;
using System.Collections.Generic;
using System.Linq;
using Interactables;
using UnityEngine;

namespace CheckpointUtils
{
    public class CheckpointManager
    {
        private CheckpointOriginator _originator = null;
        private List<IMemento> _mementoes = new List<IMemento>();
    
        public CheckpointManager(CheckpointOriginator originator)
        {
            _originator = originator;
        }

        // call from checkpoint collider
        public void Backup(Vector2 checkpointLoc, Vector2 playerRespawnPoint, Obstacle obstacle = null)
        {
            _mementoes.Add(_originator.Save(checkpointLoc, playerRespawnPoint, obstacle));
            ShowHistory();
        }

        // call from player death
        public void Undo()
        {
            if (_mementoes.Count == 0)
            {
                return;
            }

            var memento = _mementoes.Last();
            _mementoes.Remove(memento);

            try
            {
                _originator.Restore(memento);
            }
            catch (Exception e)
            {
                Undo();
            }
        }

        public void ShowHistory()
        {
            Debug.Log("Checkpoint Locals: ");
            foreach (var memento in _mementoes)
            {
                Debug.Log(memento.GetCheckpointInfo().PlayerRespawnLoc + " ");
            }
        }
    }
}