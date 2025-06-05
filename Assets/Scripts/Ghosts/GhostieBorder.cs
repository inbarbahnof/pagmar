using UnityEngine;

namespace Ghosts
{
    public class GhostieBorder : MonoBehaviour
    {
        [SerializeField] private Transform _gotoPos;

        public Transform GetGoToPos()
        {
            return _gotoPos;
        }
    }
}
