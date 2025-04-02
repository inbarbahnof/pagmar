using System;
using UnityEngine;

public class EndPath : MonoBehaviour
{
    [SerializeField] private PathTarget _pathTarget;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Dog") && _pathTarget.getIsDogOnPath())
        {
            print("in EndPath");
            _pathTarget.FinishTargetAction();
        }
    }
}
