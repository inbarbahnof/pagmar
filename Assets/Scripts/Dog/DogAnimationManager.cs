using System;
using UnityEngine;

namespace Dog
{
    public class DogAnimationManager : MonoBehaviour
    {
        [SerializeField] private GameObject art;

        private DogActionManager _actionManager;
        private Vector3 lastPosition;
        private float moveXPrevDir;

        private void Start()
        {
            _actionManager = GetComponent<DogActionManager>();
            lastPosition = transform.position;
        }

        private void Update()
        {
            Vector3 currentPosition = transform.position;
            float dirX = currentPosition.x - lastPosition.x;

            if (Mathf.Sign(dirX) != Mathf.Sign(moveXPrevDir) && _actionManager.CurState != DogState.Push)
            {
                art.transform.localScale = new Vector3(
                    -art.transform.localScale.x,
                    art.transform.localScale.y,
                    art.transform.localScale.z
                );
            }

            moveXPrevDir = dirX;
            lastPosition = currentPosition;
        }
    }
}