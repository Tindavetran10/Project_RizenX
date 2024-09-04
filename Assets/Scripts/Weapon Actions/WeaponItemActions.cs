using Character.Player;
using Items;
using Items.Weapons;
using UnityEngine;

namespace Weapon_Actions
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Test Action")]
    public class WeaponItemActions : ScriptableObject
    {
        public int actionId;
    
        public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            // What does every weapon action have in common?
            // 1. We should always keep track of which weapon is currently being used
            if (playerPerformingAction.IsOwner)
            {
                playerPerformingAction.playerNetworkManager.currentWeaponBeingUsed.Value =
                    weaponPerformingAction.itemID;
            }
            
            Debug.Log("AttemptToPerformAction");
        }
    }
}