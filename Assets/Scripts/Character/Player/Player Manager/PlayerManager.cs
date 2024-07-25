namespace Character.Player.Player_Manager
{
    public class PlayerManager : CharacterManager
    {
        //private PlayerLocomotionManager _playerLocomotionManager;
        protected override void Awake()
        {
            base.Awake();

            //_playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        }
        
        protected override void Update()
        {
            base.Update();
         
            // Handle all player related methods
            //_playerLocomotionManager.HandleAllMovement();
        }
    }
}