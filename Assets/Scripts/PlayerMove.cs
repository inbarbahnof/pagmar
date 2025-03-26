using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _speed = 6f;
    
    private Vector2 _moveInput = Vector2.zero;
    private Rigidbody2D _playerRb;
    
    void Start()
    {
        _playerRb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 movement = _playerRb.position + _moveInput * (_speed * Time.fixedDeltaTime);
        _playerRb.MovePosition(movement);
    }

    public void UpdateMoveInput(Vector2 moveInput)
    {
        _moveInput = moveInput;
    }
}
