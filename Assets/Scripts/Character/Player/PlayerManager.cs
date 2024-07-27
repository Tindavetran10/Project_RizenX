namespace Character.Player
{
    public class PlayerManager : CharacterManager
    {
        private PlayerLocomotionManager _playerLocomotionManager;
        protected override void Awake()
        {
            base.Awake();
            _playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        }
        
        protected override void Update()
        {
            base.Update();
            
            // If this is not the owner of the character, don't update or edit
            if(!IsOwner) return;
         
            // Handle all player related methods
            _playerLocomotionManager.HandleAllMovement();
        }

        protected override void LateUpdate()
        {
            if (!IsOwner) return;
            base.LateUpdate();
            PlayerCamera.Instance.HandleAllCameraActions();
        }
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner) PlayerCamera.Instance.playerManager = this;
        }
    }
}