using Audio.FMOD;
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
                    AudioManager.Instance.SetParameter(AudioManager.Instance.musicInstance,
                        "Intensity", 1, false);
                }
            }
            else if (other.CompareTag("Dog"))
            {
                bool isProtected = other.GetComponent<DogActionManager>().IsDogProtected;
                // print("dog protected " + isProtected);
                if (!isProtected)
                {
                    _attack.Attack(other.transform);
                    AudioManager.Instance.SetParameter(AudioManager.Instance.musicInstance,
                        "Intensity", 1, false);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Dog") || other.CompareTag("Player"))
                AudioManager.Instance.SetParameter(AudioManager.Instance.musicInstance,
                "Intensity", 0, false);
        }
    }
}