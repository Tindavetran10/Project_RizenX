using PlayerInputManager = Character.Player.Player_Manager.PlayerInputManager;

namespace Character.Player.Player_States
{
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) {}

        public override void Awake() {}

        public override void Enter(){}

        public override void Update()
        {
            if(!PlayerStateMachine.IsOwner)
                return;
            if(PlayerInputManager.Instance.moveAmount == 0f)
                PlayerStateMachine.SwitchState(new PlayerIdleState(PlayerStateMachine));
            
            HandleAllMovement();
            
            if(PlayerStateMachine == null) return;
            
            // Why do we pass 0 as the first parameter? 
            // Because we only want non-strafe movement,
            
            // We only use the horizontal when we are strafing or locked on
            PlayerStateMachine.playerAnimatorManager.UpdateAnimatorMovementParameters(0,  
                moveAmount);
            
            // If we are locked on, pass the horizontal movement
        }

        public override void Exit() {}
    }
}