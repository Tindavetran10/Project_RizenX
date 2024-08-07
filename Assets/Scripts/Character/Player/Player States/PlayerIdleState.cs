using Character.Player.Player_Manager;

namespace Character.Player.Player_States
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) {}

        public override void Awake() {}

        public override void Enter() {}

        public override void Update()
        {
            if(!PlayerStateMachine.IsOwner) return;
            if(PlayerInputManager.Instance.moveAmount != 0f)
                PlayerStateMachine.SwitchState(new PlayerMoveState(PlayerStateMachine));
            
            if(PlayerStateMachine == null) return;
            PlayerStateMachine.playerAnimatorManager.UpdateAnimatorMovementParameters(0, 0);
        }

        public override void Exit() {}
    }
}