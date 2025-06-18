using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ghosts
{
    public class Cage : MonoBehaviour
    {
        [SerializeField] private Transform _cageGoTo;
        public event Action OnGhostEnterCage;
        
        private int _ghostsInCage;
        public int GhostiesInCage => _ghostsInCage;
        
        private List<Transform> _ghostiesInCage = new List<Transform>();
        private Dictionary<Transform, Transform> _originalParents = new Dictionary<Transform, Transform>();
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ghostie"))
            {
                Vector3 randomOffset = 
                    new Vector3(UnityEngine.Random.Range(-0.2f, 0.2f), 0f, 0f);
                
                GhostieDie die = other.GetComponent<GhostieDie>();
                GameObject ghostie = die.GetGhostie();
                
                // turn the cage into ghostie parent
                Transform ghostieTransform = ghostie.transform;
                _originalParents[ghostieTransform] = ghostieTransform.parent;
                
                ghostieTransform.SetParent(transform);
                _ghostiesInCage.Add(ghostieTransform);
                
                die.Die(transform.position + randomOffset, this);
                
                _ghostsInCage++;
            }
        }

        public void InvokeMethod()
        {
            OnGhostEnterCage?.Invoke();
        }

        public void ResetCage()
        {
            _ghostsInCage = 0;
            
            // turn parents of all ghosties in cage back to original
            foreach (Transform ghostie in _ghostiesInCage)
            {
                if (_originalParents.TryGetValue(ghostie, out Transform originalParent))
                {
                    ghostie.SetParent(originalParent);
                }
            }

            _ghostiesInCage.Clear();
            _originalParents.Clear();
        }
    }
}
