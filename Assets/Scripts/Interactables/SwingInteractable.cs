using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


namespace Interactabels
{
    public class SwingInteractable : BaseInteractable
    {
        [SerializeField] private Transform attachLoc;
        private Coroutine swing;
        private float swingTime = 3f;
        private float startAngle;

        public Transform AttachLoc => attachLoc;

        public override void Interact()
        {
            base.Interact();
            SwingInteractableManager.instance.TryStartSwing(this);
        }

        public override Vector2 GetCurPos()
        {
            return attachLoc.position;
        }

        public void StartSwing()
        {
            startAngle = -45;
            swing = StartCoroutine(SwingOverTime());
        }

        // rotate rope gameobj (with pivot on top)
        private IEnumerator SwingOverTime()
        {
            float elapsedTime = 0f;

            while (elapsedTime < swingTime)
            {
                float currentAngle = Mathf.Lerp(startAngle, -startAngle, elapsedTime / swingTime);
                transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = Quaternion.Euler(0f, 0f, -startAngle);
            FinishSwing();
        }

        private void FinishSwing()
        {
            // at finish anim
            // reposition player at end
            SwingInteractableManager.instance.FinishSwing(this);
        }

        public override void StopInteractPress()
        {
            // stop anim and drop
            SwingInteractableManager.instance.StopSwing(this);
            // fall anim
        }

        public void ResetSwing()
        {
            if (swing != null) StopCoroutine(swing);
            FinishInteraction();
        }


    }
}
