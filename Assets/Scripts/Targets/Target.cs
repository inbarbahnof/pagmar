using System;
using System.Collections;
using UnityEngine;

namespace Targets
{
    public class Target : MonoBehaviour
    {
        [SerializeField] protected GameObject _dogTempComunication;
        [SerializeField] private float distance = 0.6f;
        [SerializeField] protected bool isTOI = true;

        public event Action OnTargetActionComplete;
        public bool IsTOI => isTOI;

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
            if (_dogTempComunication != null)
                _dogTempComunication.SetActive(false);
            
            OnTargetActionComplete?.Invoke();
        }
    }
}
