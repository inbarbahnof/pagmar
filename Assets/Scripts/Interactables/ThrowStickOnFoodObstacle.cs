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

        [Header("Ghost Appearance")] 
        [SerializeField] private GameObject _ghost;

        public void DropStick()
        {
            _food.transform.DOMove(_dropFoodPos.position, _dropDuration)
                .SetEase(Ease.OutBounce);
            
            _feedDogObstacle.HandleFoodDroppedWalkable(_food);
            _food.FoodCanBeFed();

            // TODO make noise and bring the ghost
            if (_ghost != null)  _ghost.SetActive(true);
        }

        void Update()
        {

        }
    }
}
