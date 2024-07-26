using UnityEngine;
using UnityEngine.SceneManagement;
using World_Manager;

namespace Character.Player.Player_Manager
{
    public class PlayerInputManager : MonoBehaviour
    {
        //Goals:
        // 1. Read input from player
        // 2. Move the player based on the input

        public static PlayerInputManager Instance { get; private set; }

        private PlayerController _playerController;

        [SerializeField] private Vector2 movementInput;
        [SerializeField] public float verticalInput;
        [SerializeField] public float horizontalInput;
        [SerializeField] public float moveAmount;

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
            }
            _playerController.Enable();
        }

        private void OnDestroy()
        {
            // When the game object is destroyed, stop the OnSceneChanged method
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }
        
        private void Update() => HandleMovementInput();

        private void HandleMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;
            
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
            
            switch (moveAmount)
            {
                case <= 0.5f and > 0f: moveAmount = 0.5f; break;
                case > 0.5f and <= 1f: moveAmount = 1f; break;
            }
        }
    }
}
