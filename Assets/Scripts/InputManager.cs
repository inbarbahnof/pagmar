using Interactabels;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerMove _player;
    private PlayerStateManager _stateManager;
    
    void Start()
    {
        _player = GetComponent<PlayerMove>();
        _stateManager = GetComponent<PlayerStateManager>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        _player.UpdateMoveInput(moveInput);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            InteractableManager.instance.OnInteract();  // Button just pressed
        }
        else if (context.canceled)
        {
            InteractableManager.instance.OnStopInteract(); // Button released
        }
    }

    public void OnCall(InputAction.CallbackContext context)
    {
        if (context.started) _stateManager.SetState(PlayerState.Call);
    }
}
