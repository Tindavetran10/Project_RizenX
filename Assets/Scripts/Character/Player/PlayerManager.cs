using System.Collections;
using System.Linq;
using Character.Player.Player_UI;
using Game_Saving;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using World_Manager;

namespace Character.Player
{
    public class PlayerManager : CharacterManager
    {
        [Header("Debug Menu")]
        [SerializeField] private bool respawnCharacter;
        [SerializeField] private bool switchRightWeapon;
        [SerializeField] private bool isUsingWeapon;
        
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatManager playerStatManager;
        [HideInInspector] public PlayerInventoryManager playerInventoryManager;
        [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
        [HideInInspector] public PlayerCombatManager playerCombatManager;
        
        protected override void Awake()
        {
            base.Awake();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatManager = GetComponent<PlayerStatManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            
            isUsingWeapon = false;
        }
        
        protected override void Update()
        {
            base.Update();
            
            // If this is not the owner of the character, don't update or edit
            if(!IsOwner) return;
         
            // Handle all player related methods
            playerLocomotionManager.HandleAllMovement();
            
            // Regenerate stamina
            playerStatManager.RegenerateStamina();
            
            // Debug Menu
            DebugMenu();
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
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
            
            if (IsOwner)
            {
                PlayerCamera.Instance.playerManager = this;
                PlayerInputManager.Instance.PlayerManager = this;
                WorldSaveGameManager.Instance.playerManager = this;
                
                // Update the total amount of health and stamina based on the changes of vitality and endurance level
                playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;
                
                // Update the UI stat bars when a stat changes (Health or Stamina)
                playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.Instance.playerUIHudManager.SetNewHealthValue;
                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.Instance.playerUIHudManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatManager.ResetStaminaRegenTimer;
            }
            
            // Stats
            playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHP;
            
            // Lock On
            playerNetworkManager.isLockedOn.OnValueChanged += playerNetworkManager.OnIsLockedOnChange;
            playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged += playerNetworkManager.OnLockTargetIDChange;
            
            // Equipment
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            playerNetworkManager.currentLeftHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            playerNetworkManager.currentWeaponBeingUsed.OnValueChanged += playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;

            // Upon connecting, if we are the owner of this character,
            // But we are not the server, reload our character data to this newly instantiated character,
            // We don't run this if we are the server, because since they are the host, they are already loaded in and don't need to reload their data
            if (IsOwner && !IsServer) LoadGameDataFromCurrentCharacterData(ref WorldSaveGameManager.Instance.currentCharacterData);
        }

        private void OnClientConnectedCallback(ulong clientID)
        {
            // keep a list of active players in the games
            WorldGameSessionManager.instance.AddPlayerToActivePlayerList(this);
            
            // If we are the server, we are the host, so we don't need to load in our character data, we are already loaded in
            // You only need to load in your character data if you are a client
            if (!IsServer && IsOwner)
            {
                foreach (var player in WorldGameSessionManager.instance.players.Where(player => player != this)) 
                    player.LoadOtherPlayerCharacterWhenJoiningServer();
            }
        }
        
        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner) 
                PlayerUIManager.Instance.playerUIPopUpManager.SendYouDiedPopUp();
            
            // Check for Players that are alive, if 0 then end the game
            
            return base.ProcessDeathEvent(manuallySelectDeathAnimation);
        }

        protected override void ReviveCharacter()
        {
            base.ReviveCharacter();
            
            if(IsOwner)
            {
                isDead.Value = false;
                playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
                playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;
                // Restore Focus points
                
                // Player respawn effect
                playerAnimatorManager.PlayTargetActionAnimation("Empty", true);
            }
        }

        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            
            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.zPosition = transform.position.z;
            
            currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
            currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value ;
            
            currentCharacterData.vitality = playerNetworkManager.vitality.Value;
            currentCharacterData.endurance = playerNetworkManager.endurance.Value;
        }
        
        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;
            var myPosition = new Vector3 (currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
            transform.position = myPosition;
            
            playerNetworkManager.vitality.Value = currentCharacterData.vitality;
            playerNetworkManager.endurance.Value = currentCharacterData.endurance;
            
            // This will be moved when saving and loading is added
            // Set max health and stamina based on the loaded values
            playerNetworkManager.maxHealth.Value = playerStatManager.CalculateHealthBasedOnVitalityLevel(playerNetworkManager.vitality.Value);
            playerNetworkManager.maxStamina.Value = playerStatManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            
            // Set the current health and stamina after max values are set
            playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth > 0 ? currentCharacterData.currentHealth : playerNetworkManager.maxHealth.Value;
            //playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
            playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
            
            PlayerUIManager.Instance.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
        }

        private void LoadOtherPlayerCharacterWhenJoiningServer()
        {
            // Make sure when the player joins the server, they load in the other player's using weapon status
            playerNetworkManager.OnIsUsingWeaponChange(playerNetworkManager.isUsingWeapon.Value);
            
            // Sync the other player's weapon ID's
            playerNetworkManager.OnCurrentRightHandWeaponIDChange(0, playerNetworkManager.currentRightHandWeaponID.Value);
            playerNetworkManager.OnCurrentLeftHandWeaponIDChange(0, playerNetworkManager.currentLeftHandWeaponID.Value);
            
            // Sync the other player's amor ID's 
            
            // Lock on status
            if(playerNetworkManager.isLockedOn.Value) 
                playerNetworkManager.OnLockTargetIDChange(0, playerNetworkManager.currentTargetNetworkObjectID.Value);
        }
        
        // Debug Delete Later
        private void DebugMenu()
        {
            if (respawnCharacter)
            {
                respawnCharacter = false;
                ReviveCharacter();
            }
            
            if (switchRightWeapon)
            {
                switchRightWeapon = false;
                
                // Custom: Switch between using a weapon and not using a weapon
                isUsingWeapon = !isUsingWeapon;
                
                playerAnimatorManager.UpdateAnimatorWeaponParameters(isUsingWeapon);
                playerEquipmentManager.SwitchRightWeapon();
            }
        }
    }
}