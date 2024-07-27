using UnityEngine;

namespace Character.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance { get; private set; }
        public PlayerManager playerManager;
        public Camera cameraObject;
        
        // Change these to tweak camera performance
        [Header("Camera Settings")] 
        
        // The bigger the value, the slower the camera will follow the player
        private const float CameraSmoothSpeed = 1f;
        [SerializeField] private float leftAndRightLookSpeed = 220f;
        [SerializeField] private float upAndDownLookSpeed = 220f;
        [SerializeField] private float minimumPivot = -30f; // Minimum angle the camera can look down
        [SerializeField] private float maximumPivot = 60f; // Maximum angle the camera can look up
        
        [Header("Camera Values")] 
        private Vector3 _cameraVelocity;
        [SerializeField] private float leftAndRightLookAngle;
        [SerializeField] private float upAndDownLookAngle;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else Destroy(gameObject);
        }
        
        private void Start() => DontDestroyOnLoad(gameObject);
        
        public void HandleAllCameraActions()
        {
            if (playerManager != null)
            {
                HandleFollowTarget();
            }
            
            // Follow the Player
            // Rotate around the Player
            // Collide with the environment
        }

        private void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, 
                playerManager.transform.position, 
                ref _cameraVelocity, 
                CameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }
        
        private void HandleRotates()
        {
            // If locked on, force rotation towards target,
            // If not locked on, rotates around the player
            
            // Normal Rotations
            leftAndRightLookAngle += (PlayerInputManager.Instance.cameraHorizontalInput * leftAndRightLookSpeed) * Time.deltaTime;
            upAndDownLookAngle -= (PlayerInputManager.Instance.cameraVerticalInput * upAndDownLookSpeed) * Time.deltaTime;
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);
        }
    }
}