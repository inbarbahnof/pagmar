using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshManager : MonoBehaviour
{
    public static NavMeshManager instance;
    private NavMeshSurface Surface2D;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Debug.LogError("TOO MANY PUSH NAVMESH MANAGERS!");

        Surface2D = GetComponent<NavMeshSurface>();
        Surface2D.BuildNavMeshAsync();
    }

    public void ReBake()
    {
        print("rebake");
        Surface2D.UpdateNavMesh(Surface2D.navMeshData);
    }
}
