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

        private PlayerController _playerController;

        [Header("Player Movement Input")]
        [SerializeField] private Vector2 movementInput;
        [SerializeField] public float verticalInput;
        [SerializeField] public float horizontalInput;
        
        // Combine Vertical Input and Horizontal Input to get the move direction
        [SerializeField] public float moveAmount;
        
        [Header("Camera Movement Input")]
        [SerializeField] private Vector2 cameraInput;
        [SerializeField] public float cameraVerticalInput;
        [SerializeField] public float cameraHorizontalInput;
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
        }

        private void OnSceneChanged(Scene oldScene, Scene newScene)
        {
            // If we load into the new scene, enable the PlayerInputManager
            if(newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex())
                Instance.enabled = true;
            // Otherwise, disable the PlayerInputManager
            else Instance.enabled = false;
        }

        private void OnEnable()
        {
            if(_playerController == null)
            {
                _playerController = new PlayerController();
            
                // give the input value from the Movement in the PlayerController to the movementInput
                _playerController.PlayerMovement.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
                _playerController.PlayerCamera.Movement.performed += ctx => cameraInput = ctx.ReadValue<Vector2>();

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

        private void Update()
        {
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
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
        }

        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }
    }
}