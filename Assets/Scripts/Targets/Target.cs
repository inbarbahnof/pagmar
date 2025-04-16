using System;
using System.Collections;
using UnityEngine;

namespace Targets
{
    public class Target : MonoBehaviour
    {
        [SerializeField] private float distance = 0.6f;
        public event Action OnTargetActionComplete;

        public float GetDistance()
        {
            return distance;
        }

        // started the action
        public virtual void StartTargetAction()
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
