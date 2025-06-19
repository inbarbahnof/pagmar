using Dog;
using Interactables;
using UnityEngine;
using UnityEngine.Serialization;

namespace Targets
{
    public class WaitToRunTarget : Target
    {
        [SerializeField] private bool _isLast;
        [SerializeField] private Stealth2Obstacle _manager2;
        [SerializeField] private Stealth3Obstacle _manager3;

        public override void StartTargetAction(PlayerFollower dog)
        {
            if (_manager2 != null) _manager2.TargetReached(_isLast);
            
            if (_manager3 != null) _manager3.TargetReached(_isLast);
            
            dog.GetComponent<DogActionManager>().ChangeCrouching(true);
            // if (_isLast) FinishTargetAction();
        }
        
    }
}