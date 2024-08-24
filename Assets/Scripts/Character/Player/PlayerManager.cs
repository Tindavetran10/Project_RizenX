using Character.Player.Player_UI;
using Game_Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
using World_Manager;

namespace Character.Player
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatManager playerStatManager;
        protected override void Awake()
        {
            base.Awake();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatManager = GetComponent<PlayerStatManager>();
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
            
            playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHP;
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
    }
}