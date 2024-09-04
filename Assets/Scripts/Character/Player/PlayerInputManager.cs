using UnityEngine;
using UnityEngine.SceneManagement;
using World_Manager;

namespace Character.Player
{
    public class PlayerInputManager : MonoBehaviour
    {
        //Goals:
        // 1. Read input from player
        // 2. Move the player based on the input

        public static PlayerInputManager Instance { get; private set; }
        public PlayerManager PlayerManager { get; set; }
        private PlayerController _playerController;

        [Header("Camera Movement Input")]
        [SerializeField] private Vector2 cameraInput;
        [SerializeField] public float cameraVerticalInput;
        [SerializeField] public float cameraHorizontalInput;
        
        [Header("Player Movement Input")]
        [SerializeField] private Vector2 movementInput;
        [SerializeField] public float verticalInput;
        [SerializeField] public float horizontalInput;
        
        // Combine Vertical Input and Horizontal Input to get the move direction
        [SerializeField] public float moveAmount;
        
        [Header("Player Actions Input")]
        [SerializeField] private bool dodgeInput;
        [SerializeField] private bool sprintInput;
        [SerializeField] private bool jumpInput;
        [SerializeField] private bool rbInput;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        
            // When the scene changes, run the OnSceneChanged method
            SceneManager.activeSceneChanged += OnSceneChanged;      
        
            Instance.enabled = false;

            _playerController?.Disable();
        }   

        private void OnSceneChanged(Scene oldScene, Scene newScene)
        {
            // If we load into the new scene, enable the PlayerInputManager
            if(newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex())
            {
                Instance.enabled = true;

                _playerController?.Enable();
            }
            // Otherwise, disable the PlayerInputManager
            else
            {
                Instance.enabled = false;
                _playerController?.Disable();
            }
        }

        private void OnEnable()
        {
            if(_playerController == null)
            {
                _playerController = new PlayerController();
            
                // give the input value from the Movement in the PlayerController to the movementInput
                _playerController.PlayerMovement.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
                _playerController.PlayerCamera.Movement.performed += ctx => cameraInput = ctx.ReadValue<Vector2>();
                _playerController.PlayerActions.Dodge.performed += ctx => dodgeInput = true;
                _playerController.PlayerActions.Jump.performed += ctx => jumpInput = true;
                
                _playerController.PlayerActions.RB.performed += ctx => rbInput = true;
                
                // Holding the sprint button, set sprintInput to true
                _playerController.PlayerActions.Sprint.performed += ctx => sprintInput = true;
                // When the sprint button is released, set sprintInput to false
                _playerController.PlayerActions.Sprint.canceled += ctx => sprintInput = false;
                
            }
            _playerController.Enable();
        }

        private void OnDestroy()
        {
            // When the game object is destroyed, stop the OnSceneChanged method
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (enabled)
            {
                if(hasFocus)
                    _playerController.Enable();
                else _playerController.Disable();
            }
        }

        private void Update() => HandleALLInput();

        private void HandleALLInput()
        {
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HandleSprintInput();
            HandleJumpInput();
            
            HandleRBInput();
        }

        // This method will handle the movement input from the player,
        // not the movement speed
        private void HandlePlayerMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;
            
            // Return the absolute value of the horizontal and vertical input
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

            switch (moveAmount)
            {
                // We clamp the moveAmount to 0.5 or 1
                case <= 0.5f and > 0f:
                    moveAmount = 0.5f;
                    break;
                case > 0.5f and <= 1f:
                    moveAmount = 1f;
                    break;
            }
            // Why do we only pass 0 on the horizontal input?
            // Because we are only using the horizontal input when we are strafing or locked on to an enemy
            
            if(PlayerManager == null) return;
            
            // If we are not strafing or locked on to an enemy, we only use the moveAmount
            PlayerManager.playerAnimatorManager.UpdateAnimatorMovementParameters(0, 
                moveAmount, PlayerManager.playerNetworkManager.isSprinting.Value);
            
            // If we are strafing or locked on to an enemy, we use the horizontal input as well
        }

        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }
        
        private void HandleDodgeInput()
        {
            // If the player presses the dodge button, dodge
            if (dodgeInput)
            {
                dodgeInput = false;
                
                // Future note: do nothing if UI is open
                // Perform the dodge action
                PlayerManager.playerLocomotionManager.AttemptToPerformDodge();
            }
        }

        private void HandleSprintInput()
        {
            if (sprintInput) PlayerManager.playerLocomotionManager.HandleSprinting();
            else PlayerManager.playerNetworkManager.isSprinting.Value = false;
        }

        private void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInput = false;
                
                // If we have a UI open, do nothing
                
                // attempt to perform the jump action
                PlayerManager.playerLocomotionManager.AttemptToPerformJump();
            }
        }
        
        private void HandleRBInput()
        {
            if (rbInput)
            {
                rbInput = false;
                
                // If we have a UI open, do nothing
                
                // attempt to perform the right hand action
                PlayerManager.playerNetworkManager.SetCharacterActionHand(true);
                
                // If we are two handing the weapon, use the two-handed action
                
                PlayerManager.playerCombatManager.PerformWeaponBaseAction(
                    PlayerManager.playerInventoryManager.currentRightHandWeapon.oh_rb_Action, 
                    PlayerManager.playerInventoryManager.currentRightHandWeapon);
            }
        }
    }
}
