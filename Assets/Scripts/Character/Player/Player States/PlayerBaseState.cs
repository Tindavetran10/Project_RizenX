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
        private Vector3 _targetRotationDirection;
        
        protected readonly PlayerStateMachine StateMachine;
        protected PlayerBaseState(PlayerStateMachine stateMachine) => StateMachine = stateMachine;

        private void GetVerticalAndHorizontalInput()
        {
            _verticalMovement = PlayerInputManager.Instance.verticalInput;
            _horizontalMovement = PlayerInputManager.Instance.horizontalInput;
            moveAmount = PlayerInputManager.Instance.moveAmount;
        }

        protected void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
        }

        private void HandleGroundedMovement()
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

        private void HandleRotation()
        {
            _targetRotationDirection = Vector3.zero;
            _targetRotationDirection = PlayerCamera.Instance.cameraObject.transform.forward * _verticalMovement;
            _targetRotationDirection += PlayerCamera.Instance.cameraObject.transform.right * _horizontalMovement;
            _targetRotationDirection.Normalize();
            _targetRotationDirection.y = 0;
            
            if(_targetRotationDirection == Vector3.zero) 
                _targetRotationDirection = StateMachine.transform.forward;
            
            var newRotation = Quaternion.LookRotation(_targetRotationDirection);
            var targetRotation = Quaternion.Slerp(
                StateMachine.transform.rotation, 
                newRotation, StateMachine.RotationSpeed * Time.deltaTime);
            
            StateMachine.transform.rotation = targetRotation;
        }
    }
}