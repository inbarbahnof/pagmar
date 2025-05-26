using Interactables;
using UnityEngine;
using UnityEngine.Serialization;

namespace Targets
{
    public class WaitToRunTarget : Target
    {
        [SerializeField] private bool _isLast;
        [SerializeField] private DogStealthDistractionObsManager _manager;

        public override void StartTargetAction()
        {
            if (_isLast) FinishTargetAction();
            else _manager.TargetReached();
        }
        
    }
}