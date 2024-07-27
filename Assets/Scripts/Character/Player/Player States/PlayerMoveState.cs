using UnityEngine.InputSystem;
using PlayerInputManager = Character.Player.Player_Manager.PlayerInputManager;

namespace Character.Player.Player_States
{
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) {}

        public override void Awake() {}

        public override void Enter(){}

        public override void Tick(float deltaTime)
        {
            if(!StateMachine.IsOwner)
                return;
            if(PlayerInputManager.Instance.moveAmount == 0f)
                StateMachine.SwitchState(new PlayerIdleState(StateMachine));
            
            HandleAllMovement();
        }

        public override void Exit() {}
    }
}