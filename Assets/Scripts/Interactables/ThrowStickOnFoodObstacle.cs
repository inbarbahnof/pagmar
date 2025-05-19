using DG.Tweening;
using UnityEngine;

namespace Interactables
{ 
    public class ThrowStickOnFoodObstacle : MonoBehaviour
    {
        [SerializeField] private FoodPickUpInteractable _food;
        [SerializeField] private Vector3 _dropFoodPos;
        [SerializeField] private float _dropDuration = 1f;

        // void Start()
        // {
        //     _food.OnDroppedOnWalkableSurface += HandleFoodDroppedWalkable;
        // }
        //
        //
        // private void HandleFoodDroppedWalkable(FoodPickUpInteractable food)
        // {
        //     
        // }

        public void DropStick()
        {
            _food.transform.DOMove(_dropFoodPos, _dropDuration)
                .SetEase(Ease.OutBounce);
            
            // TODO make noise and bring the ghost
        }

        void Update()
        {

        }
    }
}
