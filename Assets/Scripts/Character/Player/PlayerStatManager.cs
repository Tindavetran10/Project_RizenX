namespace Character.Player
{
    public class PlayerStatManager : CharacterStatManager
    {
        private PlayerManager _playerManager;
        
        protected override void Awake()
        {
            base.Awake();
            _playerManager = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();
            
            // Why do we calculate these values here?
            // When we make a character creation menu and set the stats depending on the class,
            // this will be calculated there.
            // Until then, however, stats are never calculated, so we do it here at the start,
            // if a save file exists, they will be overwritten when loading a new scene.
            
            CalculateHealthBasedOnVitalityLevel(_playerManager.playerNetworkManager.vitality.Value);
            CalculateStaminaBasedOnEnduranceLevel(_playerManager.playerNetworkManager.endurance.Value);
        }
    }
}