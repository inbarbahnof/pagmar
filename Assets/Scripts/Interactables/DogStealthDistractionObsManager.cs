using System;
using Dog;
using Ghosts;
using Targets;
using UnityEngine;

namespace Interactables
{
    public class DogStealthDistractionObsManager : Obstacle
    {
        [SerializeField] private DogActionManager _dog;
        [SerializeField] private Target[] _targets;
        [SerializeField] private GhostMovement[] _ghosts;
        [SerializeField] private ThrowablePickUpInteractable[] _sticks;

        private int _curTarget = 0;


        public override void ResetObstacle()
        {
            _dog.transform.position = _targets[0].transform.position;
            
            // reset stick positions
            
            // reset ghost positions
            
            // reset player position
        }
    }
}
