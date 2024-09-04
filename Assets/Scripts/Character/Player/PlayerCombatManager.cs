using Items;
using Items.Weapons;
using Unity.Netcode;
using Weapon_Actions;

namespace Character.Player
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        private PlayerManager _playerManager;
        public WeaponItem currentWeaponBeingUsed;
        
        protected override void Awake()
        {
            base.Awake();
            _playerManager = GetComponent<PlayerManager>();
        }
        
        public void PerformWeaponBaseAction(WeaponItemActions weaponAction, WeaponItem weaponPerformingAction)
        {
            if (_playerManager.IsOwner)
            {
                // Perform the weapon action
                weaponAction.AttemptToPerformAction(_playerManager, weaponPerformingAction);
                
                // Notify the server we have performed an action, so we perform it from there perspective as well
                _playerManager.playerNetworkManager.NotifyTheServerOfWeaponActionServerRpc(
                    NetworkManager.Singleton.LocalClientId, weaponAction.actionId, weaponPerformingAction.itemID);
            }
            
        }
    }
}