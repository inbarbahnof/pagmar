using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshManager : MonoBehaviour
{
    public static NavMeshManager instance;
    [SerializeField] private NavMeshSurface Surface2D;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Debug.LogError("TOO MANY PUSH NAVMESH MANAGERS!");
        
        Surface2D.BuildNavMeshAsync();
    }

    public void ReBake()
    {
        Surface2D.UpdateNavMesh(Surface2D.navMeshData);
    }
}
