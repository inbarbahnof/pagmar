using System;
using System.Collections;
using UnityEngine;
using PathCreation;
using Targets;
using UnityEngine.AI;

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
            distanceTravelled += speed * Time.deltaTime;
            float pathLength = pathCreator.path.length;
            
            if (distanceTravelled < pathLength)
            {
                Vector3 pos = pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
                transform.position = pos;
            }
            else
            {
                _target.FinishTargetAction();
                Vector3 pos = pathCreator.path.GetPointAtDistance(pathLength, EndOfPathInstruction.Stop);
                transform.position = pos;
                
                var agent = GetComponent<NavMeshAgent>();
                if (agent != null) agent.Warp(pos);
                
                isOnPath = false;
                distanceTravelled = 0;
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
