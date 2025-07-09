using System;
using UnityEngine;

namespace Interactables
{
    public class ThrowStickOnTreeDetector : Obstacle
    {
        [SerializeField] private ThrowStickOnFoodObstacle _throwStickOnFoodObstacle;
        [SerializeField] private Animator _treeAnimator;
        [SerializeField] private ParticleSystem _particle;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Stick"))
            {
                if (other.GetComponent<ThrowablePickUpInteractable>().IsThrowing)
                {
                    _throwStickOnFoodObstacle.DropStick();
                    _treeAnimator.SetTrigger("hit");
                    _particle.Play();
                }
            }
        }

        public override void ResetObstacle()
        {
            // TODO reset?
        }
    }
}
