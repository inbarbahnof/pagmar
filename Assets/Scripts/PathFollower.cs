using System;
using UnityEngine;
using PathCreation;

public class PathFollower : MonoBehaviour
{
    public float speed = 3f;
    private float distanceTravelled;
    private bool isOnPath = false;
    private PathCreator pathCreator;

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
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        // transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
    }

    public void SetCurPath(PathCreator path)
    {
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
