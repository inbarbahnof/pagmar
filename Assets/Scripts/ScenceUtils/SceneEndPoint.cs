using UnityEngine;

public class SceneEndPoint : MonoBehaviour
{
    public void EndScene()
    {
        GameManager.instance.LevelEnd();
    }
}