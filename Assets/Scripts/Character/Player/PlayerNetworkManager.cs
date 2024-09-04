using Character.Player.Player_UI;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Weapon_Actions;

namespace Character.Player
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        private PlayerManager _playerManager;
        
        public NetworkVariable<FixedString64Bytes> characterName = new("Character", 
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Equipment")] 
        public NetworkVariable<int> currentWeaponBeingUsed =
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        public NetworkVariable<int> currentRightHandWeaponID =
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        public NetworkVariable<int> currentLeftHandWeaponID =
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        public NetworkVariable<bool> isUsingRightHand = 
            new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        public NetworkVariable<bool> isUsingLeftHand = 
            new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        protected override void Awake()
        {
            base.Awake();
            _playerManager = GetComponent<PlayerManager>();
        }

        public void SetCharacterActionHand(bool rightHandAction)
        {
            if (rightHandAction)
            {
                isUsingRightHand.Value = true;
                isUsingLeftHand.Value = false;
            }
            else
            {
                isUsingRightHand.Value = false;
                isUsingLeftHand.Value = true;
            }
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
        
        public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newID)
        {
            var newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponItemByID(newID));
            _playerManager.playerCombatManager.currentWeaponBeingUsed = newWeapon;
        }
        
        // Item Actions
        [ServerRpc]
        public void NotifyTheServerOfWeaponActionServerRpc(ulong clientId , int actionID, int weaponID)
        {
            if(IsServer)
                NotifyTheServerOfWeaponActionClientRpc(clientId, actionID, weaponID);
        }
        
        [ClientRpc]
        private void NotifyTheServerOfWeaponActionClientRpc(ulong clientId , int actionID, int weaponID)
        {
            if(clientId != NetworkManager.Singleton.LocalClientId)
                PerformWeaponBaseAction(actionID, weaponID);
        }

        private void PerformWeaponBaseAction(int actionID, int weaponID)
        {
            WeaponItemActions weaponAction = WorldActionManager.instance.GetWeaponItemActionByID(actionID);

            if (weaponAction != null)
            {
                weaponAction.AttemptToPerformAction(_playerManager, 
                    WorldItemDatabase.instance.GetWeaponItemByID(weaponID));
            }
            else Debug.Log("Weapon Action is null");
        }
    }
}