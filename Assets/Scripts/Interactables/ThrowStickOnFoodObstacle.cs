using System;
using System.Collections;
using Audio.FMOD;
using DG.Tweening;
using Targets;
using Unity.VisualScripting;
using UnityEngine;

namespace Interactables
{ 
    public class ThrowStickOnFoodObstacle : MonoBehaviour
    {
        [SerializeField] private FoodPickUpInteractable _food;
        [SerializeField] private ThrowablePickUpInteractable _stick;
        [SerializeField] private FeedDogObstacle _feedDogObstacle;
        [SerializeField] private Transform _dropFoodPos;
        [SerializeField] private Transform _dropStickPos;
        [SerializeField] private float _dropfoodDuration = 1f;
        [SerializeField] private float _droprockDuration = 1f;

        [Header("Ghost Appearance")] 
        [SerializeField] private Stealth1Obstacle _stealth;
        [SerializeField] private ParticleSystem _particle;
        
        private Rigidbody2D stickRb;
        private Rigidbody2D foodRb;

        private void Start()
        {
            _food.SetCanInteract(false);
            foodRb = _food.GetComponent<Rigidbody2D>();
        }

        public void DropStick()
        {
            foodRb.bodyType = RigidbodyType2D.Dynamic;
            foodRb.gravityScale = 1f;
            foodRb.linearVelocity = CalculateDropVelocity(_food.transform.position, 
                _dropFoodPos.position, _droprockDuration);

            StartCoroutine(WaitToDropStick());
            
            _feedDogObstacle.HandleFoodDroppedWalkable(_food);
            _food.FoodCanBeFed();
            
            _food.SetCanInteract(false);
            _stick.SetCanInteract(false);
            
            TargetGenerator.instance.NotifyFoodNearby(
                _food.GetComponent<FoodTargetGetter>().GetFoodTarget());

            _stealth.GhostAppear();
        }

        private IEnumerator WaitToDropStick()
        {
            yield return new WaitForSeconds(0.2f);
            
            if (!stickRb)
            {
                stickRb = _stick.gameObject.AddComponent<Rigidbody2D>();
                stickRb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            }

            stickRb.bodyType = RigidbodyType2D.Dynamic;
            stickRb.gravityScale = 1f;
            stickRb.linearVelocity = CalculateDropVelocity(_stick.transform.position,
                _dropStickPos.position, _droprockDuration - 0.2f);

            StartCoroutine(TurnOffRigidBody(stickRb, false));
        }
        
        private Vector2 CalculateDropVelocity(Vector2 from, Vector2 to, float duration)
        {
            Vector2 velocity = (to - from) / duration;
            velocity.y += 0.5f * Mathf.Abs(Physics2D.gravity.y) * duration;
            return velocity;
        }

        private IEnumerator TurnOffRigidBody(Rigidbody2D rb, bool isFood)
        {
            if (isFood) yield return new WaitForSeconds(_dropfoodDuration);
            else yield return new WaitForSeconds(_droprockDuration);
            
            rb.gravityScale = 0f;
            rb.angularVelocity = 0;
            rb.linearVelocity = Vector2.zero;
            
            if (isFood) _particle.Play();
        }
    }
}
