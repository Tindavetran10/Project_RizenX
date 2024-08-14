using UnityEngine;

namespace Character.Player
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        private PlayerManager _playerManager;
        
        // These values will be transferred from the PlayerInputManager
        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float moveAmount;

        [Header("Movement Settings")]
        private Vector3 _moveDirection;
        private Vector3 _targetRotationDirection;
        [SerializeField] private float walkingSpeed;
        [SerializeField] private float runningSpeed;
        [SerializeField] private float rotationSpeed;
        
        [Header("Dodge Settings")]
        private Vector3 _rollDirection;
        
        protected override void Awake()
        {
            base.Awake();
            _playerManager = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (_playerManager.IsOwner)
            {
                _playerManager.characterNetworkManager.verticalMovement.Value = verticalMovement;
                _playerManager.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
                _playerManager.characterNetworkManager.moveAmount.Value = moveAmount;
            }
            else
            {
                horizontalMovement = _playerManager.characterNetworkManager.horizontalMovement.Value;
                verticalMovement = _playerManager.characterNetworkManager.verticalMovement.Value;
                moveAmount = _playerManager.characterNetworkManager.moveAmount.Value;
                
                // If not locked on, pass the moveAmount
                _playerManager.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
                
                // If locked on, pass the horizontal and vertical movement
            }
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
        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.Instance.verticalInput;
            horizontalMovement = PlayerInputManager.Instance.horizontalInput;
            moveAmount = PlayerInputManager.Instance.moveAmount;
        }

        private void HandleGroundedMovement()
        {
            // Pass the vertical and horizontal movement to moveDirection
            GetMovementValues();
            
            if(_playerManager.canMove == false) return;
            
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
            if(_playerManager.canRotate == false) return;
            
            _targetRotationDirection = Vector3.zero;
            _targetRotationDirection = PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
            _targetRotationDirection += PlayerCamera.Instance.cameraObject.transform.right * horizontalMovement;
            _targetRotationDirection.Normalize();
            _targetRotationDirection.y = 0;
            
            if(_targetRotationDirection == Vector3.zero)
                _targetRotationDirection = transform.forward;
            
            var newRotation = Quaternion.LookRotation(_targetRotationDirection);
            var targetRotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
            transform.rotation = targetRotation;
        }

        public void AttemptToPerformDodge()
        {
            if(_playerManager.isPerformingAction) return;
            
            // If we are moving and the dodge input is true, we perform a roll
            if(PlayerInputManager.Instance.moveAmount > 0)
            {
                _rollDirection = PlayerCamera.Instance.cameraObject.transform.forward * PlayerInputManager.Instance.verticalInput;
                _rollDirection += PlayerCamera.Instance.cameraObject.transform.right * PlayerInputManager.Instance.horizontalInput;
                _rollDirection.y = 0;
                _rollDirection.Normalize();
                
                var playerRotation = Quaternion.LookRotation(_rollDirection);
                _playerManager.transform.rotation = playerRotation;
                
                // Perform a roll animation
                _playerManager.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true, true);
            }
            // If we are not moving, we perform a backstep
            else
            {
                // Perform a backstep animation
            }
        }
    }
}