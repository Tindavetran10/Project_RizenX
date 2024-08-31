using UnityEngine;

namespace Character
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        private CharacterManager _characterManager;

        [Header("Ground Check & Jumping")]
        [SerializeField] protected float gravityForce = -5.55f; // The force at which our character is pulled down
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckSphereRadius = 0.5f; // The radius of the sphere that checks if the character is grounded
        [SerializeField] protected Vector3 yVelocity; // The force at which our character is pulled up or down
        [SerializeField] protected float groundedYVelocity = -20f; // The force at which our character is sticking to the ground whilst they are grounded
        [SerializeField] protected float fallStartYVelocity = -5f; // The force at which our character starts falling
        private bool _fallingVelocityHasBeenSet = false;
        private float _inAirTimer;
        
        private static readonly int InAirTimer = Animator.StringToHash("InAirTimer");

        protected virtual void Awake() => 
            _characterManager = GetComponent<CharacterManager>();

        protected virtual void Update()
        {
            HandleGroundCheck();

            if (_characterManager.isGrounded)
            {
                // If we are not attempting to jump or move upwards,
                // set the yVelocity to the groundedVelocity
                if(yVelocity.y < 0f)
                {
                    _inAirTimer = 0f;
                    _fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                // If we are not jumping and our falling velocity has not been set,
                if(!_characterManager.isJumping && !_fallingVelocityHasBeenSet)
                {
                    _fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }

                _inAirTimer += Time.deltaTime;
                _characterManager.animator.SetFloat(InAirTimer, _inAirTimer);
                yVelocity.y += gravityForce * Time.deltaTime;
            }
            
            // THere should always be a small amount of yVelocity to keep the character grounded
            _characterManager.characterController.Move(yVelocity * Time.deltaTime);
        }

        private void HandleGroundCheck()
        {
            _characterManager.isGrounded = Physics.CheckSphere(_characterManager.transform.position, 
                groundCheckSphereRadius, groundLayer);
        }

        // Draw a red wire sphere around the character to show the ground check radius
        protected void OnDrawGizmosSelected()
        {
            if (_characterManager == null) 
                _characterManager = GetComponent<CharacterManager>();

            if (_characterManager != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(_characterManager.transform.position, groundCheckSphereRadius);
            }
        }
    }
}