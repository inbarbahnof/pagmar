using System;
using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float distance = 0.1f;
    public event Action OnTargetActionComplete;

    public float GetDistance()
    {
        return distance;
    }

    // started the action
    public virtual void StartTargetAction()
    {
        StartCoroutine(HoverTarget());
    }

    private IEnumerator HoverTarget()
    {
        yield return new WaitForSeconds(2f);
        FinishTargetAction();
    }
    
    // finished the action
    public virtual void FinishTargetAction()
    {
        OnTargetActionComplete?.Invoke();
    }
}
