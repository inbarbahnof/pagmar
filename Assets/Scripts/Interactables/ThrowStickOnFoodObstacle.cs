using DG.Tweening;
using UnityEngine;

namespace Interactables
{ 
    public class ThrowStickOnFoodObstacle : MonoBehaviour
    {
        [SerializeField] private FoodPickUpInteractable _food;
        [SerializeField] private FeedDogObstacle _feedDogObstacle;
        [SerializeField] private Transform _dropFoodPos;
        [SerializeField] private float _dropDuration = 1f;

        public void DropStick()
        {
            _food.transform.DOMove(_dropFoodPos.position, _dropDuration)
                .SetEase(Ease.OutBounce);
            
            _feedDogObstacle.HandleFoodDroppedWalkable(_food);
            _food.FoodCanBeFed();

            // TODO make noise and bring the ghost
        }

        void Update()
        {

        }
    }
}
