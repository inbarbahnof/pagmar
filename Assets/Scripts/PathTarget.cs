using PathCreation;
using UnityEngine;

public class PathTarget : Target
{
    [SerializeField] private PathFollower follower;
    [SerializeField] private PathCreator pathCreator;

    private bool isDogOnPath;
    
    public override void StartTargetAction()
    {
        follower.SetCurPath(pathCreator);
        isDogOnPath = true;
        follower.SetIsOnPath(true);
    }
    
    public override void FinishTargetAction()
    {
        follower.SetIsOnPath(false);
        isDogOnPath = false;
        base.FinishTargetAction();
    }

    public bool getIsDogOnPath()
    {
        return isDogOnPath;
    }
}
