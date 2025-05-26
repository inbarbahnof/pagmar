using System;
using Dog;
using PathCreation;
using UnityEngine;

namespace Targets
{
    public class PathTarget : Target
    {
        public static event Action<bool> OnDogProtectionChanged;
        
        [SerializeField] private PathFollower follower;
        [SerializeField] private PathCreator pathCreator;
        [SerializeField] private bool _isStealthPath;

        private bool isDogOnPath;

        public override void StartTargetAction()
        {
            isDogOnPath = true;
            follower.SetCurPath(pathCreator, this);
            follower.SetIsOnPath(isDogOnPath);
            
            if (_isStealthPath) OnDogProtectionChanged?.Invoke(true);
        }

        public override void FinishTargetAction()
        {
            isDogOnPath = false;

            if (_isStealthPath)
            {
                OnDogProtectionChanged?.Invoke(false);
                TargetGenerator.instance.SetStealthTarget(null);
            }
            
            base.FinishTargetAction();
        }

        public bool getIsDogOnPath()
        {
            return isDogOnPath;
        }
    }
}
