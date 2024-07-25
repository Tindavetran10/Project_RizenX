using State_Machines;
using UnityEngine;
using PlayerInputManager = Character.Player.Player_Manager.PlayerInputManager;

namespace Character.Player.Player_States
{
    public abstract class PlayerBaseState : State
    {
        public float verticalMovement;
        public float horizontalMovement;
        public float moveAmount;
        
        public Vector3 moveDirection;
        [SerializeField] private float _walkingSpeed;
        [SerializeField] private float _runningSpeed;
        
        
        protected readonly PlayerStateMachine StateMachine;
        protected PlayerBaseState(PlayerStateMachine stateMachine) => StateMachine = stateMachine;

        private void HandleGroundedMovement()
        {
            moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
            moveDirection += PlayerCamera.Instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;

            if (PlayerInputManager.Instance.moveAmount > 0.5f)
            {
                StateMachine.characterController.Move(moveDirection * _runningSpeed * Time.deltaTime);
            }
            else if (PlayerInputManager.Instance.moveAmount <= 0.5f)
            {
                StateMachine.characterController.Move(moveDirection * _walkingSpeed * Time.deltaTime);
            }
        }
    }
}