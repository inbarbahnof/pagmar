using Dog;
using PathCreation;
using UnityEngine;

namespace Targets
{
    public class PathTarget : Target
    {
        [SerializeField] private PathFollower follower;
        [SerializeField] private PathCreator pathCreator;

        private bool isDogOnPath;

        public override void StartTargetAction()
        {
            isDogOnPath = true;
            follower.SetCurPath(pathCreator);
            follower.SetIsOnPath(isDogOnPath);
        }

        public override void FinishTargetAction()
        {
            isDogOnPath = false;
            follower.SetIsOnPath(isDogOnPath);
            follower.ResetFollow();
            base.FinishTargetAction();
        }

        public bool getIsDogOnPath()
        {
            return isDogOnPath;
        }
    }
}
