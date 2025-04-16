using System;
using UnityEngine;

public class PushInteractable : BaseInteractable
{
    [SerializeField] private bool _needDogToPush = false;
    
    private Transform _player;
    private Transform _dog;
    private PlayerMove _playerMove;
    private bool _isBeingPushed = false;
    private float _xOffset;
    private DogActionManager _dogAction;
    private Vector3 _dogOffsetFromObject;

    [SerializeField] private float pushSpeed = 5f;

    public override void Interact()
    {
        // base.Interact(player, dog);
        //
        // _player = player;
        // _isBeingPushed = true;
        //
        // _playerMove = _player.GetComponent<PlayerMove>();
        // _playerMove.SetIsPushing(true);
        //
        // _xOffset = transform.position.x - player.position.x;
        //
        // if (_needDogToPush)
        // {
        //     _dog = dog;
        //     _dogAction = dog.GetComponent<DogActionManager>();
        //     _dogOffsetFromObject = transform.position - dog.position;
        // }
    }

    public override void StopInteractPress()
    {
        _isBeingPushed = false;
        _player = null;
        
        _playerMove.SetIsPushing(false);
        _playerMove = null;

        _dog = null;
        _dogAction = null;
        
        FinishInteraction();
    }

    private void Update()
    {
        if (_isBeingPushed && _player != null)
        {
            if (_needDogToPush)
            {
                if (_dogAction.CurState == DogState.Push)
                {
                    PushObject();
                    PushDog();
                }
            }
            else
            {
                PushObject();  
            }
        }
    }

    private void PushDog()
    {
        Vector3 targetPos = transform.position - _dogOffsetFromObject;
        _dog.position = targetPos;
        // _dog.position = Vector3.Lerp(_dog.position, targetPos, Time.deltaTime * pushSpeed);
    }

    private void PushObject()
    {
        Vector3 newPos = transform.position;
        newPos.x = _player.position.x + _xOffset;
        transform.position = newPos;
    }
}
