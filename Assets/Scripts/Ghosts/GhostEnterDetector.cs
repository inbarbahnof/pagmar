using UnityEngine;

namespace Ghosts
{
    public class GhostEnterDetector : MonoBehaviour
    {
        [SerializeField] private GhostAttack _attack;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) 
            {
                if (!other.GetComponent<PlayerStealthManager>().isProtected)
                {
                    _attack.Attack(other.transform);
                }
            }
        }
    }
}