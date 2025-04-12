using UnityEngine;

public class PushInteractable : BaseInteractable
{
    private Transform _player;
    private PlayerMove _playerMove;
    private bool _isBeingPushed = false;
    private float _xOffset;

    [SerializeField] private float pushSpeed = 5f;

    public override void Interact(Transform player)
    {
        base.Interact(player);

        _player = player;
        _isBeingPushed = true;
        
        _playerMove = _player.GetComponent<PlayerMove>();
        _playerMove.SetIsPushing(true);
        
        _xOffset = transform.position.x - player.position.x;
    }

    public override void StopInteractPress()
    {
        _isBeingPushed = false;
        _player = null;
        
        _playerMove.SetIsPushing(false);
        _playerMove = null;
        
        FinishInteraction();
    }

    private void Update()
    {
        if (_isBeingPushed && _player != null)
        {
            Vector3 newPos = transform.position;
            newPos.x = _player.position.x + _xOffset;
            transform.position = newPos;
        }
    }
}
