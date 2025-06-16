using UnityEngine;

namespace Dog
{
    public class DogStopRunning : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Dog"))
            {
                other.GetComponent<PlayerFollower>().SetSpeed(true);
            }
        }
    }
}