using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using World_Manager;

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
        [SerializeField] private float cameraCollisionRadius = 0.2f;
        [SerializeField] private LayerMask collideWithLayers;
        
        [Header("Camera Values")] 
        private Vector3 _cameraVelocity;
        
        // The value used for camera collision
        // (Move the camera object to this position upon collision)
        private Vector3 _cameraObjectPosition; 
        
        [SerializeField] private float leftAndRightLookAngle;
        [SerializeField] private float upAndDownLookAngle;
        
        private float _cameraZPosition; // The value used for camera collision
        private float _targetCameraZPosition; // The value used for camera collision
        
        [Header("Lock On")]
        [SerializeField] private float lockOnRadius = 26f;
        [SerializeField] private float minimumViewableAngle = -50f;
        [SerializeField] private float maximumViewableAngle = 50f;
        private readonly List<CharacterManager> _availableTargets = new List<CharacterManager>();
        public CharacterManager nearestLockOnTarget;
        
        private void Awake()
        {
            if (Instance == null)
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
            // Follow the Player
            // Rotate around the Player
            // Collide with the environment
            
            if (playerManager is not null)
            {
                HandleFollowTarget();
                HandleRotates();
                HandleCollisions();
            }
        }

        private void HandleFollowTarget()
        {
            var targetCameraPosition = Vector3.SmoothDamp(transform.position, 
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
            leftAndRightLookAngle += PlayerInputManager.Instance.cameraHorizontalInput * leftAndRightLookSpeed * Time.deltaTime;
            // Rotate up and down based on the vertical input
            upAndDownLookAngle -= PlayerInputManager.Instance.cameraVerticalInput * upAndDownLookSpeed * Time.deltaTime;
            // Clamp the up and down look angle to the minimum and maximum pivot
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);
            
            var cameraRotation = Vector3.zero;

            // Rotate this game object left and right
            cameraRotation.y = leftAndRightLookAngle;
            var targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;
            
            // Rotate the camera pivot up and down
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }
        
        private void HandleCollisions()
        {
            // If the camera collides with the environment, move the camera to the closest point
            // If the camera is not colliding with the environment,
            // move the camera to the target position
            
            _targetCameraZPosition = _cameraZPosition;
            // Get the direction for collision check
            var direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();
            
            // We check if there is an object in front of our desired position (see the direction above)
            if(Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out var hit, 
                   Mathf.Abs(_cameraZPosition), collideWithLayers))
            {
                // if there is, we get our distance from it
                var distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                // we then equate our target z position to the following
                _targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
            }
            
            // If our target position is less than the camera collision radius, we subtract our collision radius
            // (making it snap back)
            if(Mathf.Abs(_targetCameraZPosition) < cameraCollisionRadius) 
                _targetCameraZPosition = -cameraCollisionRadius;
            
            // we then apply our final position using lerp over a time of 0.2f
            _cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, _targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = _cameraObjectPosition;
        }

        public void HandleLocationLockOnTarget()
        {
            float shortestDistance = Mathf.Infinity; // will be used to find the closest target
            float shortestDistanceOfRightTarget = Mathf.Infinity; // will be used to find the closest target on one axis to the right of the current target
            float shortestDistanceOfLeftTarget = -Mathf.Infinity; // will be used to find the closest target on one axis to the left of the current target
            
            var colliders = Physics.OverlapSphere(playerManager.transform.position, lockOnRadius, 
                WorldUtilityManager.Instance.GetCharacterLayers());

            foreach (var character in colliders)
            {
                var lockOnTargets = character.GetComponent<CharacterManager>();

                if (lockOnTargets != null)
                {
                    // Check if they are within our field of view
                    var lockOnTargetDirection = lockOnTargets.transform.position - playerManager.transform.position;
                    var distanceFromTarget = Vector3.Distance(playerManager.transform.position, 
                        lockOnTargets.transform.position);
                    var viewableAngle = Vector3.Angle(lockOnTargetDirection, cameraObject.transform.forward);
                    
                    if(lockOnTargets.isDead.Value) continue; // Skip dead targets
                    
                    if(lockOnTargets.transform.root == playerManager.transform.root) continue; // Accidentally lock on to yourself
                    
                    // Lastly, if the target is outside of our viewable angle, skip it
                    if(viewableAngle > minimumViewableAngle && viewableAngle < maximumViewableAngle)
                    {
                        // TODO: add player mask for environment layer only
                        
                        if(Physics.Linecast(
                               playerManager.playerCombatManager.lockOnTransform.position, 
                               lockOnTargets.characterCombatManager.lockOnTransform.position, 
                               out var hit, WorldUtilityManager.Instance.GetEnviroLayers()))
                        {
                            // We hit something, we cannot see out lock on target
                            continue;
                        }
                        else _availableTargets.Add(lockOnTargets);
                    }
                    
                }
            }
            
            // We now sort through the available targets and find the closest one
            foreach (var target in _availableTargets)
            {
                if (target != null)
                {
                    var distanceFromTarget = Vector3.Distance(playerManager.transform.position, 
                        target.transform.position);
                    
                    var lockOnTargetDirection = target.transform.position - 
                                                playerManager.transform.position;
                    
                    if(distanceFromTarget < shortestDistance)
                    {
                        shortestDistance = distanceFromTarget;
                        nearestLockOnTarget = target;
                    }
                }
                else
                {
                    ClearLockOnTargets();
                    playerManager.playerNetworkManager.isLockedOn.Value = false;
                }
            }
        }
        
        public void ClearLockOnTargets()
        {
            nearestLockOnTarget = null;
            _availableTargets.Clear();
        }
    }
}