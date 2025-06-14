using UnityEngine;

namespace Interactables
{
    public class ClimbInteractableManager : MonoBehaviour
    {
        public static ClimbInteractableManager instance;

        [SerializeField] private PlayerStateManager _playerStateManager;

        private ClimbObject _curInteraction;
        
        void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else Debug.LogError("TOO MANY PUSH INTERACTABLE MANAGERS!");
        }

        public void Climb(ClimbObject cur)
        {
            _curInteraction = cur;
            _playerStateManager.UpdateClimbing();
        }

        public void StopInteraction()
        {
            _curInteraction.StopInteraction();
        }
    }
}