using Character.Player.Player_UI;
using Unity.Collections;
using Unity.Netcode;

namespace Character.Player
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        private PlayerManager _playerManager;
        
        public NetworkVariable<FixedString64Bytes> characterName = new("Character", 
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        protected override void Awake()
        {
            base.Awake();
            _playerManager = GetComponent<PlayerManager>();
        }

        public void SetNewMaxHealthValue(int oldVitality, int newVitality)
        {
            maxHealth.Value = _playerManager.playerStatManager.CalculateHealthBasedOnVitalityLevel(newVitality);
            PlayerUIManager.Instance.playerUIHudManager.SetMaxHealthValue(maxHealth.Value);
            currentHealth.Value = maxHealth.Value;
        }
        
        public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
        {
            maxStamina.Value = _playerManager.playerStatManager.CalculateStaminaBasedOnEnduranceLevel(newEndurance);
            PlayerUIManager.Instance.playerUIHudManager.SetMaxStaminaValue(maxStamina.Value);
            currentStamina.Value = maxStamina.Value;
        }
    }
}