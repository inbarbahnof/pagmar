using System;
using DG.Tweening;
using Targets;
using Unity.VisualScripting;
using UnityEngine;

namespace Interactables
{ 
    public class ThrowStickOnFoodObstacle : MonoBehaviour
    {
        [SerializeField] private FoodPickUpInteractable _food;
        [SerializeField] private FeedDogObstacle _feedDogObstacle;
        [SerializeField] private Transform _dropFoodPos;
        [SerializeField] private float _dropDuration = 1f;

        [Header("Ghost Appearance")] [SerializeField]
        private Stealth1Obstacle _stealth;

        public void DropStick()
        {
            _food.transform.DOMove(_dropFoodPos.position, _dropDuration)
                .SetEase(Ease.OutBounce);

            _food.gameObject.layer = LayerMask.NameToLayer("Default");
            
            _feedDogObstacle.HandleFoodDroppedWalkable(_food);
            _food.FoodCanBeFed();
            TargetGenerator.instance.NotifyFoodNearby(_food.GetComponent<FoodTarget>());

            _stealth.GhostAppear();
        }
    }
}
