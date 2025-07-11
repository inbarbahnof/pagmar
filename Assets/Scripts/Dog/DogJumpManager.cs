using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Dog
{
    public class DogJumpManager : MonoBehaviour
    {
        public void OnTriggerEntered(GameObject dog, bool fromLeft)
        {
            var agent = dog.GetComponent<NavMeshAgent>();
            
            Vector2 velocity = agent.velocity;
            // Determine direction dog is moving
            bool goingRight = velocity.x > 2.5f;
            bool goingLeft = velocity.x < -2.5f;
            
            // If dog is going right and touched left trigger → allow jump
            // If dog is going left and touched right trigger → allow jump
            if ((fromLeft && goingRight) || (!fromLeft && goingLeft))
            {
                dog.GetComponent<Dog.DogActionManager>().DogJump(fromLeft);
            }
        }
    }
}