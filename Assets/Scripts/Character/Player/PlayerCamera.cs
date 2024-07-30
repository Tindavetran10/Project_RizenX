using Character.Player.Player_Manager;
using Character.Player.Player_States;
using UnityEngine;

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
        [SerializeField] private float leftAndRightRotationSpeed = 100f;
        [SerializeField] private float upAndDownRotationSpeed = 100f;
        [SerializeField] private float minimumPivot = -30f;
        [SerializeField] private float maximumPivot = 60f;
        [SerializeField] private float cameraCollisionRadius = 0.2f;
        [SerializeField] private LayerMask collisionLayer;
        
        // Display the values in the inspector
        [Header("Camera Values")]
        private Vector3 _cameraVelocity;
        // Used from camera collisions (move the camera object to this position upon collision)
        private Vector3 _cameraObjectPosition; 
        [SerializeField] private float leftAndRightLookAngle;
        [SerializeField] private float upAndDownLookAngle;
        
        [Header("Camera Collision")]
        private float _cameraZPosition;
        private float _targetCameraZPosition;
        
        
        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else Destroy(gameObject);
        }
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            _cameraZPosition = cameraObject.transform.localPosition.z;
        }

        public void HandleAllCameraActions()
        {
            // Follow the player
            // Rotate the camera around the player
            // Collision detection
            
            if(PlayerStateMachine != null)
            {
                HandleFollowTarget();
                HandleRotations();
                HandleCollisions();
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
            leftAndRightLookAngle += PlayerInputManager.Instance.cameraHorizontalInput * leftAndRightRotationSpeed * Time.deltaTime;
            // Rotate up and down based on the vertical input
            upAndDownLookAngle -= PlayerInputManager.Instance.cameraVerticalInput * upAndDownRotationSpeed * Time.deltaTime;
            // Clamp the up and down look angle between the min and max value
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);
            
            var cameraRotation = Vector3.zero;

            // Rotate this game object left and right
            cameraRotation.y = leftAndRightLookAngle;
            var targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;
            
            // Rotate the pivot game object up and down
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        private void HandleCollisions()
        {
            _targetCameraZPosition = _cameraZPosition;
            // direction for collision check
            var direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();
            
            // We check if there is an object in front of our desired direction (see "direction" variable)
            if(Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, 
                   out var hit, Mathf.Abs(_targetCameraZPosition), collisionLayer))
            {
                // if there is, we get the distance from the camera pivot to the hit object
                var distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                // we then equate our target z position to the following
                _targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
            }
            
            // if our target position is less than our collision radius, we subtract the collision radius from it
            // (making it snap back)
            if(Mathf.Abs(_targetCameraZPosition) < cameraCollisionRadius) 
                _targetCameraZPosition = -cameraCollisionRadius;
            
            // we then apply or final position using lerp
            _cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, _targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = _cameraObjectPosition;
        }
    }
}