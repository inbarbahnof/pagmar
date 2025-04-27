using UnityEngine;

namespace Interactables
{
    public class Obstacle : MonoBehaviour
    {
        protected bool SetupComplete;

        public virtual void ReachedTarget()
        {
            SetupComplete = true;
        }
        
        public virtual void ResetObstacle()
        {
            
        }
    }
}
