using UnityEngine;

namespace Targets
{
    public class FoodTargetGetter : MonoBehaviour
    {
        [SerializeField] private FoodTarget _food;

        public FoodTarget GetFoodTarget()
        {
            return _food;
        }
    }
}