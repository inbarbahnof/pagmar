using Dog;
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
                // TODO why not check player state?
                if (!other.GetComponent<PlayerStealthManager>().isProtected)
                {
                    _attack.Attack(other.transform);
                }
            }
            else if (other.CompareTag("Dog"))
            {
                if (!other.GetComponent<DogActionManager>().IsDogProtected)
                {
                    _attack.Attack(other.transform);
                }
            }
        }
    }
}