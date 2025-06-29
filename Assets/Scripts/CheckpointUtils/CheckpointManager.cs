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
        private List<IMemento> _mementos = new List<IMemento>();
    
        public CheckpointManager(CheckpointOriginator originator)
        {
            _originator = originator;
        }

        // call from checkpoint collider
        public void Backup(Vector2 checkpointLoc, Vector2 playerRespawnPoint, Obstacle obstacle = null)
        {
            _mementos.Add(_originator.Save(checkpointLoc, playerRespawnPoint, obstacle));
            //ShowHistory();
        }

        // call from player death
        public void Undo()
        {
            if (_mementos.Count == 0)
            {
                return;
            }

            var memento = _mementos.Last();
            _mementos.Remove(memento);
            //Debug.Log("undo to: " + memento.GetCheckpointInfo().PlayerRespawnLoc);

            try
            {
                _originator.Restore(memento);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Undo();
            }
        }

        public void ShowHistory()
        {
            Debug.Log("Checkpoint Locals: ");
            foreach (var memento in _mementos)
            {
                Debug.Log(memento.GetCheckpointInfo().PlayerRespawnLoc + " ");
            }
        }
    }
}