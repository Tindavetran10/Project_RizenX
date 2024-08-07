using PlayerInputManager = Character.Player.Player_Manager.PlayerInputManager;

namespace Character.Player.Player_States
{
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) {}

        public override void Awake() {}

        public override void Enter(){}

        public override void Update()
        {
            if(!StateMachine.IsOwner)
                return;
            if(PlayerInputManager.Instance.moveAmount == 0f)
                StateMachine.SwitchState(new PlayerIdleState(StateMachine));
            
            HandleAllMovement();
            
            if(StateMachine == null) return;
            
            // Why do we pass 0 as the first parameter? 
            // Because we only want non-strafe movement,
            
            // We only use the horizontal when we are strafing or locked on
            StateMachine.playerAnimatorManager.UpdateAnimatorMovementParameters(0,  
                moveAmount);
            
            // If we are locked on, pass the horizontal movement
        }

        public override void Exit() {}
    }
}