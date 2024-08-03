using UnityEngine;

namespace Character.Player
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        private PlayerManager _playerManager;
        
        // These values will be transferred from the PlayerInputManager
        public float verticalMovement;
        public float horizontalMovement;
        public float moveAmount;

        [SerializeField] private float walkingSpeed;
        [SerializeField] private float runningSpeed;
        [SerializeField] private float rotationSpeed;

        private Vector3 _moveDirection;
        private Vector3 _targetRotationDirection;
        
        protected override void Awake()
        {
            base.Awake();
            _playerManager = GetComponent<PlayerManager>();
        }
        
        public void HandleAllMovement()
        {
            // Handle all movement related methods
            // Grounded Movement
            HandleGroundedMovement();
            HandleRotation();
            // Airborne Movement
        }
        
        // Get the vertical and horizontal movement from the PlayerInputManager
        private void GetVerticalAndHorizontalMovement()
        {
            verticalMovement = PlayerInputManager.Instance.verticalInput;
            horizontalMovement = PlayerInputManager.Instance.horizontalInput;
        }

        private void HandleGroundedMovement()
        {
            // Pass the vertical and horizontal movement to moveDirection
            GetVerticalAndHorizontalMovement();
            
            // Move direction is based on the camera's perspective and movement input
            _moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
            _moveDirection += PlayerCamera.Instance.transform.right * horizontalMovement;
            _moveDirection.Normalize();
            _moveDirection.y = 0;

            switch (PlayerInputManager.Instance.moveAmount)
            {
                case > 0.5f:
                    // Move the player at the running speed
                    _playerManager.characterController.Move(_moveDirection * (runningSpeed * Time.deltaTime));
                    break;
                case <= 0.5f:
                    // Move the player at the walking speed
                    _playerManager.characterController.Move(_moveDirection * (walkingSpeed * Time.deltaTime));
                    break;
            }
        }

        private void HandleRotation()
        {
            _targetRotationDirection = Vector3.zero;
            _targetRotationDirection = PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
            _targetRotationDirection += PlayerCamera.Instance.cameraObject.transform.right * horizontalMovement;
            _targetRotationDirection.Normalize();
            _targetRotationDirection.y = 0;
            
            if(_targetRotationDirection == Vector3.zero)
                _targetRotationDirection = transform.forward;
            
            Quaternion newRotation = Quaternion.LookRotation(_targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
            transform.rotation = targetRotation;
        }
    }
}