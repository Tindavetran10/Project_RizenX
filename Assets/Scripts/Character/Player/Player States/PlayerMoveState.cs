namespace Character.Player.Player_States
{
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) {}

        public override void Awake() {}

        public override void Enter(){}

        public override void Tick(float deltaTime) => HandleGroundedMovement();

        public override void Exit() {}
    }
}