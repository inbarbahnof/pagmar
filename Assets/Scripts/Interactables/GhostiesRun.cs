using Ghosts;
using UnityEngine;

namespace Interactables
{ 
    public class GhostiesRun : MonoBehaviour
    {
        [SerializeField] private GhostieMovement[] _ghosties;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Dog"))
            {
                foreach (var ghosty in _ghosties)
                {
                    ghosty.MoveAwayFromDog(other.transform.position);
                }
            }
        }
    }
}
