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

        private Vector3 _moveDirection;
        
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

            if (PlayerInputManager.Instance.moveAmount > 0.5f)
            {
                // Move the player at the running speed
                _playerManager.characterController.Move(_moveDirection * (runningSpeed * Time.deltaTime));
            }
            else if (PlayerInputManager.Instance.moveAmount <= 0.5f)
            {
                // Move the player at the walking speed
                _playerManager.characterController.Move(_moveDirection * (walkingSpeed * Time.deltaTime));
            }
            
        }
    }
}