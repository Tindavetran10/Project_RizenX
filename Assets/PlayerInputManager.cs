using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    
    //Goals:
    // 1. Read input from the player
    // 2. Move the player based on the input
    
    PlayerController _playerController;

    [SerializeField] private Vector2 movementInput;

    private void OnEnable()
    {
        if(_playerController == null)
        {
            _playerController = new PlayerController();
            
            // give the input value from the Movement in the PlayerController to the movementInput
            _playerController.PlayerMovement.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        }
        _playerController.Enable();
    }
}
