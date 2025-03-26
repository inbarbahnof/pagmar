using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerMove _player;
    
    void Start()
    {
        _player = GetComponent<PlayerMove>();    
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        _player.UpdateMoveInput(moveInput);
    }
}
