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
        [SerializeField] private float jumpHeight;
        [SerializeField] private float walkingSpeed;
        [SerializeField] private float runningSpeed;
        [SerializeField] private float sprintingSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float sprintingStaminaCost;
        
        [Header("Dodge Settings")]
        private Vector3 _rollDirection;
        [SerializeField] private float dodgeStaminaCost;
        [SerializeField] private float jumpStaminaCost;
        
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
                _playerManager.playerAnimatorManager.UpdateAnimatorMovementParameters(0, 
                    moveAmount, _playerManager.playerNetworkManager.isSprinting.Value);
                
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
            if(!_playerManager.canMove) return;
            
            // Pass the vertical and horizontal movement to moveDirection
            GetMovementValues();
            
            // Move direction is based on the camera's perspective and movement input
            _moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
            _moveDirection += PlayerCamera.Instance.transform.right * horizontalMovement;
            _moveDirection.Normalize();
            _moveDirection.y = 0;
            
            if (_playerManager.playerNetworkManager.isSprinting.Value)
                // Move the player at the running speed
                _playerManager.characterController.Move(_moveDirection * (sprintingSpeed * Time.deltaTime));
            else
            {
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
        }

        private void HandleRotation()
        {
            if(!_playerManager.canRotate) return;
            
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

        public void HandleSprinting()
        {
            if (_playerManager.isPerformingAction)
                // Set sprinting to false   
                _playerManager.playerNetworkManager.isSprinting.Value = false;
            
            // If we are out of stamina, set sprinting to false
            if(_playerManager.playerNetworkManager.currentStamina.Value <= 0)
            {
                _playerManager.playerNetworkManager.isSprinting.Value = false;
                return;
            }
            
            // If we are moving fast enough, set sprinting to true
            if(moveAmount >= 0.5f)
                _playerManager.playerNetworkManager.isSprinting.Value = true;
            else
                // If we are not moving, set sprinting to false
                _playerManager.playerNetworkManager.isSprinting.Value = false;
            
            // Reduce stamina if we are sprinting
            if(_playerManager.playerNetworkManager.isSprinting.Value)
                // Drain stamina
                _playerManager.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
            
            
            // If we are stationary, set sprinting to false
        }
        
        public void AttemptToPerformDodge()
        {
            if(_playerManager.isPerformingAction) return;
            
            if(_playerManager.playerNetworkManager.currentStamina.Value <= 0) return;
            
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
                _playerManager.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward", true);
            }
            // If we are not moving, we perform a backstep
            else
                // Perform a backstep animation
                _playerManager.playerAnimatorManager.PlayTargetActionAnimation("Back_Step", true);
            
            // Drain stamina on dodge
            _playerManager.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;
        }
        
        public void AttemptToPerformJump()
        {
            // If we are performing a general action, we don't want to allow a jump
            // (will change if combat actions are added)
            if(_playerManager.isPerformingAction) return;
            
            // If we are out of stamina, we don't want to allow a jump
            if(_playerManager.playerNetworkManager.currentStamina.Value <= 0) return;
            
            // If we are jumping, we don't want to allow another jump until the current jump is finished
            if(_playerManager.isJumping) return;
            
            // If we are not grounded, we don't want to allow a jump
            if(!_playerManager.isGrounded) return;
            
            // If we are two handing a weapon, play the two-handed jump animation instead
            // Otherwise, play the one-handed jump animation
            _playerManager.playerAnimatorManager.PlayTargetActionAnimation("Main_Jump_01", false);
            
            _playerManager.isJumping = true;
            
            // Drain stamina on dodge
            _playerManager.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;
        }

        public void ApplyJumpingVelocity()
        {
            // Apply the upward velocity to the character controller
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityForce);
        }
    }
}