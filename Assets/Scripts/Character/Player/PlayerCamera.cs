using UnityEngine;

namespace Character.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance { get; private set; }
        public PlayerManager playerManager;
        public Camera cameraObject;
        [SerializeField] private Transform cameraPivotTransform;
        
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
                HandleRotates();
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
            // Rotate left and right based on the horizontal input
            leftAndRightLookAngle += (PlayerInputManager.Instance.cameraHorizontalInput * leftAndRightLookSpeed) * Time.deltaTime;
            // Rotate up and down based on the vertical input
            upAndDownLookAngle -= (PlayerInputManager.Instance.cameraVerticalInput * upAndDownLookSpeed) * Time.deltaTime;
            // Clamp the up and down look angle to the minimum and maximum pivot
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);
            
            var cameraRotation = Vector3.zero;
            Quaternion targetRotation;
            
            // Rotate this game object left and right
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;
            
            // Rotate the camera pivot up and down
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }
    }
}