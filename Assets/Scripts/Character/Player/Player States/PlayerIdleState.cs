using Character.Player.Player_Manager;

namespace Character.Player.Player_States
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) {}

        public override void Awake() {}

        public override void Enter() {}

        public override void Tick(float deltaTime)
        {
            if(!StateMachine.IsOwner)
                return;
            if(PlayerInputManager.Instance.moveAmount != 0f)
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
        }

        public override void Exit() {}
    }
}