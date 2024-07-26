using System;
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
        
        protected readonly PlayerStateMachine StateMachine;
        protected PlayerBaseState(PlayerStateMachine stateMachine) => StateMachine = stateMachine;

        private void GetVerticalAndHorizontalInput()
        {
            verticalMovement = PlayerInputManager.Instance.verticalInput;
            horizontalMovement = PlayerInputManager.Instance.horizontalInput;
            moveAmount = PlayerInputManager.Instance.moveAmount;
        }

        protected void HandleGroundedMovement()
        {
            GetVerticalAndHorizontalInput();
            
            moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
            moveDirection += PlayerCamera.Instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;

            switch (PlayerInputManager.Instance.moveAmount)
            {
                case > 0.5f:
                    StateMachine.characterController.Move(moveDirection * (StateMachine.RunningSpeed * Time.deltaTime));
                    break;
                case <= 0.5f:
                    StateMachine.characterController.Move(moveDirection * (StateMachine.WalkingSpeed * Time.deltaTime));
                    break;
            }
        }
    }
}