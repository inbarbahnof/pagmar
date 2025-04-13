using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SwingInteractable : BaseInteractable
{
    [SerializeField] private Transform attachLoc;
    private Transform _player;
    private Coroutine swing;
    private float swingTime = 5f;
    private float startAngle;
    private Quaternion _ogPlayerRot;
    private float _ogPlayerYPos;

    
    public override void Interact(Transform player)
    {
        base.Interact(player);
        // jump and swing anim
        _player = player;
        AttachPlayerToRope();
        StartSwing();
    }

    public override Vector2 GetCurPos()
    {
        return attachLoc.position;
    }

    private void AttachPlayerToRope()
    {
        _ogPlayerRot = _player.rotation;
        _ogPlayerYPos = _player.position.y;
        _player.SetParent(attachLoc);
        UpdatePlayerPos();
    }

    private void UpdatePlayerPos()
    {
        _player.position = attachLoc.position;
        _player.rotation = attachLoc.rotation;
    }

    private void StartSwing()
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
        DetachAndReset();
    }
    
    public override void StopInteractPress()
    {
        // get player cur pos
        // stop anim and drop
        DetachAndReset();
        // fall anim
    }

    private void DetachAndReset()
    {
        if (_player)
        {
            _player.SetParent(null);
            _player.rotation = _ogPlayerRot;
            _player.position = new Vector2(_player.position.x, _ogPlayerYPos);
        }
        
        _player = null;
        if (swing != null) StopCoroutine(swing);
        FinishInteraction();
    }


}
