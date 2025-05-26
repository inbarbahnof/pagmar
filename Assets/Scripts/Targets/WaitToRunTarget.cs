using UnityEngine;
using UnityEngine.Serialization;

namespace Targets
{
    public class WaitToRunTarget : Target
    {
        [SerializeField] private bool _isLast;
        public override void StartTargetAction() { }
        
        public void ThrewStick()
        {
            if (_isLast) FinishTargetAction();
            // else 
        }
    }
}