using System.Collections;
using Character.Player.Player_UI;
using Game_Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
using World_Manager;

namespace Character.Player
{
    public class PlayerManager : CharacterManager
    {
        [Header("Debug Menu")]
        [SerializeField] private bool respawnCharacter;
        [SerializeField] private bool switchRightWeapon;
        
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatManager playerStatManager;
        [HideInInspector] public PlayerInventoryManager playerInventoryManager;
        [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
        
        protected override void Awake()
        {
            base.Awake();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatManager = GetComponent<PlayerStatManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
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
            
            // Equipment
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            playerNetworkManager.currentLeftHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
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
                playerEquipmentManager.SwitchRightWeapon();
            }
        }
    }
}