using System;
using UnityEngine;
using PathCreation;
using Targets;

namespace Dog
{
    public class PathFollower : MonoBehaviour
    {
        public float speed = 3f;
        private float distanceTravelled;
        private bool isOnPath = false;
        private PathCreator pathCreator;
        private PathTarget _target;

        private void Update()
        {
            if (isOnPath)
            {
                UpdateTravelThroughPath();
            }
        }

        private void UpdateTravelThroughPath()
        {
            print("following path");
            
            distanceTravelled += speed * Time.deltaTime;
            EndOfPathInstruction instruction = EndOfPathInstruction.Stop;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, instruction);
            
            if (distanceTravelled >= pathCreator.path.length)
            {
                Debug.Log("Reached end of path!");
                _target.FinishTargetAction();
            }
        }

        public void SetCurPath(PathCreator path, PathTarget target)
        {
            _target = target;
            pathCreator = path;
        }

        public void SetIsOnPath(bool onPath)
        {
            isOnPath = onPath;
        }

        public void ResetFollow()
        {
            distanceTravelled = 0;
        }
    }
}
