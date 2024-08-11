using State_Machines;
using UnityEngine;
using PlayerInputManager = Character.Player.Player_Manager.PlayerInputManager;

namespace Character.Player.Player_States
{
    public abstract class PlayerBaseState : State
    {
        private float _verticalMovement;
        private float _horizontalMovement;
        protected float MoveAmount;

        private Vector3 _moveDirection;
        private Vector3 _targetRotationDirection;
        
        protected readonly PlayerStateMachine PlayerStateMachine;
        protected PlayerBaseState(PlayerStateMachine playerStateMachine) => PlayerStateMachine = playerStateMachine;

        public override void Update()
        {
            base.Update();
            
            if(PlayerStateMachine.IsOwner)
            {
                PlayerStateMachine.characterNetworkManager.verticalMovement.Value = _verticalMovement;
                PlayerStateMachine.characterNetworkManager.horizontalMovement.Value = _horizontalMovement;
                PlayerStateMachine.characterNetworkManager.moveAmount.Value = MoveAmount;
            }
            else
            {
                _verticalMovement = PlayerStateMachine.characterNetworkManager.verticalMovement.Value;
                _horizontalMovement = PlayerStateMachine.characterNetworkManager.horizontalMovement.Value;
                MoveAmount = PlayerStateMachine.characterNetworkManager.moveAmount.Value;
                
                // If not locked on, pass 0 as the first parameter
                PlayerStateMachine.playerAnimatorManager.UpdateAnimatorMovementParameters(0, MoveAmount);
                
                // If locked on, pass the horizontal movement
            }
        }

        private void GetMovementValues()
        {
            _verticalMovement = PlayerInputManager.Instance.verticalInput;
            _horizontalMovement = PlayerInputManager.Instance.horizontalInput;
            MoveAmount = PlayerInputManager.Instance.moveAmount;
        }

        protected void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
        }

        private void HandleGroundedMovement()
        {
            GetMovementValues();
            
            _moveDirection = PlayerCamera.Instance.transform.forward * _verticalMovement;
            _moveDirection += PlayerCamera.Instance.transform.right * _horizontalMovement;
            _moveDirection.Normalize();
            _moveDirection.y = 0;

            switch (PlayerInputManager.Instance.moveAmount)
            {
                case > 0.5f:
                    PlayerStateMachine.characterController.Move(_moveDirection * (PlayerStateMachine.RunningSpeed * Time.deltaTime));
                    break;
                case <= 0.5f:
                    PlayerStateMachine.characterController.Move(_moveDirection * (PlayerStateMachine.WalkingSpeed * Time.deltaTime));
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
                _targetRotationDirection = PlayerStateMachine.transform.forward;
            
            var newRotation = Quaternion.LookRotation(_targetRotationDirection);
            var targetRotation = Quaternion.Slerp(
                PlayerStateMachine.transform.rotation, 
                newRotation, PlayerStateMachine.RotationSpeed * Time.deltaTime);
            
            PlayerStateMachine.transform.rotation = targetRotation;
        }
    }
}