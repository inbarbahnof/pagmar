using System;
using UnityEngine;

public class SceneEndPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.LevelEnd();
    }
}