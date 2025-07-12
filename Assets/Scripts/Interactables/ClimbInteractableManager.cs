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

        public void Climb(ClimbObject cur, Vector2 playerPos, bool isClimbing, bool climbRight)
        {
            _curInteraction = cur;
            StartCoroutine(StartClimb(playerPos, isClimbing, climbRight));
        }

        public IEnumerator StartClimb(Vector2 playerPos, bool isClimbing, bool climbRight)
        {
            if (playerPos != Vector2.zero)
            {
                float waitTime = _playerMove.GetReadyToClimb(playerPos, true);
                yield return new WaitForSeconds(waitTime);
            }
            if (isClimbing) _playerStateManager.UpdateClimbing(climbRight, _curInteraction.transform);
            else _playerStateManager.UpdateDropping(climbRight, _curInteraction.transform);
        }

        public void StopInteraction()
        {
            _playerMove.FinishClimb();
        }

        public bool GetPlayerMovingRight()
        {
            return _playerMove.MovingRight;
        }
    }
}