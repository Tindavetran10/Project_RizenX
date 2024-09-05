using Character.Player;
using Items.Weapons;
using UnityEngine;

namespace Weapon_Actions
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
    public class LightAttackWeaponItemActions : WeaponItemActions
    {
        [SerializeField] private string lightAttack01 = "Main_Light_Attack_01";
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);
            
            if(!playerPerformingAction.IsOwner) return;
            
            // Check for stamina
            if(playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
                return;
            
            // Check for grounded
            if(!playerPerformingAction.isGrounded)
                return;
            
            PerformLightAttack(playerPerformingAction, weaponPerformingAction);
        }
        
        private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            
                
            if (playerPerformingAction.playerNetworkManager.isUsingRightHand.Value)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(lightAttack01, true);
            }
            if(playerPerformingAction.playerNetworkManager.isUsingLeftHand.Value)
            {
                
            }
            // Play Light Attack Animation
            // Apply Light Attack Damage
            // Apply Light Attack Effects
            // Apply Light Attack Poise Damage
            // Apply Light Attack Stamina Cost
        }
    }
}