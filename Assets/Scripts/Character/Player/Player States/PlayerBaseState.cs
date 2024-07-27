using State_Machines;
using UnityEngine;
using PlayerInputManager = Character.Player.Player_Manager.PlayerInputManager;

namespace Character.Player.Player_States
{
    public abstract class PlayerBaseState : State
    {
        private float _verticalMovement;
        private float _horizontalMovement;
        public float moveAmount;

        private Vector3 _moveDirection;
        
        protected readonly PlayerStateMachine StateMachine;
        protected PlayerBaseState(PlayerStateMachine stateMachine) => StateMachine = stateMachine;

        private void GetVerticalAndHorizontalInput()
        {
            _verticalMovement = PlayerInputManager.Instance.verticalInput;
            _horizontalMovement = PlayerInputManager.Instance.horizontalInput;
            moveAmount = PlayerInputManager.Instance.moveAmount;
        }

        protected void HandleGroundedMovement()
        {
            GetVerticalAndHorizontalInput();
            
            _moveDirection = PlayerCamera.Instance.transform.forward * _verticalMovement;
            _moveDirection += PlayerCamera.Instance.transform.right * _horizontalMovement;
            _moveDirection.Normalize();
            _moveDirection.y = 0;

            switch (PlayerInputManager.Instance.moveAmount)
            {
                case > 0.5f:
                    StateMachine.characterController.Move(_moveDirection * (StateMachine.RunningSpeed * Time.deltaTime));
                    break;
                case <= 0.5f:
                    StateMachine.characterController.Move(_moveDirection * (StateMachine.WalkingSpeed * Time.deltaTime));
                    break;
            }
        }
    }
}