using Character.Player.Player_Manager;
using Character.Player.Player_States;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance { get; private set; }
        public PlayerStateMachine PlayerStateMachine { get; set; }
        public Camera cameraObject;
        [SerializeField] private Transform cameraPivotTransform;

        // Change these values to adjust the camera performance
        [Header("Camera Settings")] 
        // The bigger the value, the slower the camera will follow the player
        private const float CameraSmoothSpeed = 0.1f;
        [FormerlySerializedAs("_leftAndRightLookSpeed")] [SerializeField] private float _leftAndRightRotationSpeed = 100f;
        [FormerlySerializedAs("_upAndDownLookSpeed")] [SerializeField] private float _upAndDownRotationSpeed = 100f;
        [SerializeField] private float minimumPivot = -30f;
        [SerializeField] private float maximumPivot = 60f;
        [SerializeField] private float cameraCollisionRadius = 0.2f;

        // Display the values in the inspector
        [Header("Camera Values")]
        private Vector3 _cameraVelocity;
        [SerializeField] private float leftAndRightLookAngle;
        [SerializeField] private float upAndDownLookAngle;
        
        [Header("Camera Collision")]
        private float defaultCameraPosition;
        private float targetCameraPosition;
        
        
        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else Destroy(gameObject);
        }
        
        private void Start() => DontDestroyOnLoad(gameObject);

        public void HandleAllCameraActions()
        {
            // Follow the player
            // Rotate the camera around the player
            // Collision detection
            
            if(PlayerStateMachine != null)
            {
                HandleFollowTarget();
                HandleRotations();
            }
        }

        private void HandleFollowTarget()
        {
            var targetCameraPosition = Vector3.SmoothDamp(
                transform.position, 
                PlayerStateMachine.transform.position, 
                ref _cameraVelocity, 
                CameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotations()
        {
            // If locked on to an enemy, rotate the camera around the enemy
            // else rotate the camera around the player
            
            // Normal Rotations
            // Rotate left and right based on the horizontal input
            leftAndRightLookAngle += PlayerInputManager.Instance.cameraHorizontalInput * _leftAndRightRotationSpeed * Time.deltaTime;
            // Rotate up and down based on the vertical input
            upAndDownLookAngle -= PlayerInputManager.Instance.cameraVerticalInput * _upAndDownRotationSpeed * Time.deltaTime;
            // Clamp the up and down look angle between the min and max value
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);
            
            var cameraRotation = Vector3.zero;
            Quaternion targetRotation;
            
            // Rotate this game object left and right
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;
            
            // Rotate the pivot game object up and down
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        private void HandleCollisions()
        {
            
        }
    }
}