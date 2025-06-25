using System;
using System.Collections;
using Dog;
using UnityEngine;

namespace Targets
{
    public class Target : MonoBehaviour
    {
        [SerializeField] private float distance = 0.6f;
        [SerializeField] protected bool isTOI = true;

        public event Action OnTargetActionComplete;
        public bool IsTOI => isTOI;

        public float GetDistance()
        {
            return distance;
        }

        // started the action
        public virtual void StartTargetAction(PlayerFollower dog)
        {
            FinishTargetAction();
        }

        // finished the action
        public virtual void FinishTargetAction()
        {
            OnTargetActionComplete?.Invoke();
        }
    }
}
