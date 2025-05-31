using Ghosts;
using UnityEngine;

namespace Interactables
{
    public class GhostiesCage1Obstacle : Obstacle
    {
        [SerializeField] private GhostieDie[] _ghosties;
        
        public override void ResetObstacle()
        {
            foreach (var ghostie in _ghosties)
            {
                ghostie.Live();
            }
        }
    }
}
