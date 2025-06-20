using System.Collections;
using UnityEngine;

namespace Interactables
{
    public class ClimbInteractableManager : MonoBehaviour
    {
        public static ClimbInteractableManager instance;

        [SerializeField] private PlayerStateManager _playerStateManager;
        private PlayerMove _playerMove;

        private ClimbObject _curInteraction;
        
        void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else Debug.LogError("TOO MANY PUSH INTERACTABLE MANAGERS!");

            _playerMove = _playerStateManager.gameObject.GetComponent<PlayerMove>();
        }

        public void Climb(ClimbObject cur, Vector2 playerPos, bool isClimbing)
        {
            _curInteraction = cur;
            if (isClimbing) StartCoroutine(StartClimb(playerPos));
            else _playerStateManager.UpdateDropping();
        }

        public IEnumerator StartClimb(Vector2 playerPos)
        {
            if (playerPos != Vector2.zero)
            {
                float waitTime = _playerMove.GetReadyToClimb(playerPos);
                yield return new WaitForSeconds(waitTime);
            }
            _playerStateManager.UpdateClimbing();
        }

        public void StopInteraction()
        {
            _playerMove.FinishClimb();
        }
    }
}