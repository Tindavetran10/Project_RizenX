namespace Character.Player.Player_States
{
    public class PlayerStateMachine : CharacterStateMachine
    {
        protected override void Start() => SwitchState(new PlayerIdleState(this));

        protected override void LateUpdate()
        {
            if(!IsOwner) return;
            base.LateUpdate();
            
            PlayerCamera.Instance.HandleAllCameraActions();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // If this is the owner of the player, then we can do stuff
            if (IsOwner)
            {
                PlayerCamera.Instance.PlayerStateMachine = this;
            }
        }
    }
}