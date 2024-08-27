using Character.Player.Player_UI;
using Items;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Character.Player
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        private PlayerManager _playerManager;
        
        public NetworkVariable<FixedString64Bytes> characterName = new("Character", 
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Equipment")] 
        public NetworkVariable<int> currentRightHandWeaponID =
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        public NetworkVariable<int> currentLeftHandWeaponID =
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
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
        
        public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)
        {
            var newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponItemByID(newID));
            _playerManager.playerInventoryManager.currentRightHandWeapon = newWeapon;
            _playerManager.playerEquipmentManager.LoadRightWeapon();
        }
        
        public void OnCurrentLeftHandWeaponIDChange(int oldID, int newID)
        {
            var newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponItemByID(newID));
            _playerManager.playerInventoryManager.currentLeftHandWeapon = newWeapon;
            _playerManager.playerEquipmentManager.LoadLeftWeapon();
        }
    }
}