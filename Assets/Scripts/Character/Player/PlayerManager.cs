using Character.Player.Player_UI;
using UnityEngine;

namespace Character.Player
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
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

                playerNetworkManager.currentStamina.OnValueChanged +=
                    PlayerUIManager.Instance.playerUIHudManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged +=
                    playerStatManager.ResetStaminaRegenTimer;
                
                
                // This will be moved when saving and loading is added
                playerNetworkManager.maxStamina.Value = playerStatManager.CalculateStaminaBasedOnLevel(playerNetworkManager.endurance.Value);
                playerNetworkManager.currentStamina.Value = playerStatManager.CalculateStaminaBasedOnLevel(playerNetworkManager.endurance.Value);
                PlayerUIManager.Instance.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
            }
        }
    }
}