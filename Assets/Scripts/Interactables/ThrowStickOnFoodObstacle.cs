using System;
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
        [SerializeField] private float _dropDuration = 1f;

        [Header("Ghost Appearance")] [SerializeField]
        private Stealth1Obstacle _stealth;

        public void DropStick()
        {
            _food.transform.DOMove(_dropFoodPos.position, _dropDuration)
                .SetEase(Ease.OutBounce);
            
            _stick.transform.DOMove(_dropStickPos.position, _dropDuration)
                .SetEase(Ease.OutBounce);
            
            _feedDogObstacle.HandleFoodDroppedWalkable(_food);
            _food.FoodCanBeFed();
            
            _food.SetCanInteract(false);
            _stick.SetCanInteract(false);
            
            TargetGenerator.instance.NotifyFoodNearby(_food.GetComponent<FoodTarget>());

            _stealth.GhostAppear();
        }
    }
}
