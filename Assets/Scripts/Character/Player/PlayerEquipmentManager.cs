using System;
using Items;
using Items.Weapons;
using UnityEngine;

namespace Character.Player
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        private PlayerManager _playerManager;
        
        public WeaponModelInstantiationSlot rightHandSlot;
        public WeaponModelInstantiationSlot leftHandSlot;
        public WeaponModelInstantiationSlot rightHipSlot;
        public WeaponModelInstantiationSlot leftHipSlot;
        public WeaponModelInstantiationSlot backSlot;

        [SerializeField] private WeaponManager rightHandWeaponManager;
        [SerializeField] private WeaponManager leftHandWeaponManager;
        
        public GameObject rightHandWeaponModel;
        public GameObject leftHandWeaponModel;

        protected override void Awake()
        {
            base.Awake();
            
            _playerManager = GetComponent<PlayerManager>();
            
            // Get our slots
            InitializeWeaponSlots();
        }

        protected override void Start()
        {
            base.Start();
            LoadWeaponOnBothHands();
        }
        
        private void InitializeWeaponSlots()
        {
            var weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();
            
            foreach (var weaponSlot in weaponSlots)
            {
                switch (weaponSlot.weaponSlot)
                {
                    case WeaponModelSlot.RightHand:
                        rightHandSlot = weaponSlot;
                        break;
                    case WeaponModelSlot.LeftHand:
                        leftHandSlot = weaponSlot;
                        break;
                    case WeaponModelSlot.RightHip:
                        rightHipSlot = weaponSlot;
                        break;
                    case WeaponModelSlot.LeftHip:
                        leftHipSlot = weaponSlot;
                        break;
                    case WeaponModelSlot.Back:
                        backSlot = weaponSlot;
                        break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void LoadWeaponOnBothHands()
        {
            LoadRightWeapon();
            LoadLeftWeapon();
        }

        #region Right Weapon
        public void LoadRightWeapon()
        {
            if (_playerManager.playerInventoryManager.currentRightHandWeapon != null)
            {
                // Remove the old weapon
                rightHandSlot.UnloadWeapon();
                
                // Bring in the new weapon
                rightHandWeaponModel = Instantiate(_playerManager.playerInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
                rightHandWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightHandWeaponManager.SetWeaponDamage(_playerManager, _playerManager.playerInventoryManager.currentRightHandWeapon);
                // Assign weapon damage to its collider
            }
        }

        public void SwitchRightWeapon()
        {
            if(!_playerManager.IsOwner) return;
            _playerManager.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", 
                true, true, true, true);
            
            // Elden Ring Weapon Swapping
            // 1. Check if we have another weapon besides our main weapon if we do, NEVER swap to unarmed, rotate between weapon 1 and weapon 2
            // 2. If we don't, swap to unarmed, then SKIP the other empty slot and swap back. Do not process both empty slots before returning to the main weapon

            WeaponItem selectedWeapon = null;
            
            // Disable two handing if we are two handing
            
            // Add one to our index to switch to the next potential weapon
            
            // Check our weapon index (we have 3 slots, so that 3 possible numbers)
            _playerManager.playerInventoryManager.rightHandWeaponIndex += 1;
            
            // If our index is greater than 2, reset it to 0
            // If our index is out of bounds, reset it to position #1 (0)
            if (_playerManager.playerInventoryManager.rightHandWeaponIndex is < 0 or > 2)
            {
                _playerManager.playerInventoryManager.rightHandWeaponIndex = 0;
                
                // We check if we are holding more than one weapon
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                var firstWeaponPosition = 0;

                for (var i = 0; i < _playerManager.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
                {
                    if(_playerManager.playerInventoryManager.weaponsInRightHandSlots[i].itemID != 
                       WorldItemDatabase.instance.unarmedWeapon.itemID)
                    {
                        weaponCount++;
                        if (firstWeapon == null)
                        {
                            firstWeapon = _playerManager.playerInventoryManager.weaponsInRightHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    _playerManager.playerInventoryManager.rightHandWeaponIndex = -1;
                    selectedWeapon = Instantiate(WorldItemDatabase.instance.unarmedWeapon);
                    _playerManager.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    _playerManager.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                    _playerManager.playerNetworkManager.currentRightHandWeaponID.Value = firstWeapon.itemID;
                }
                return;
            }

            foreach (var weapon in _playerManager.playerInventoryManager.weaponsInRightHandSlots)
            {
                // Check to see if this is not the "unarmed" weapon
                // If the next potential weapon is not the unarmed weapon, select it
                if(_playerManager.playerInventoryManager.weaponsInRightHandSlots[_playerManager.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = _playerManager.playerInventoryManager.weaponsInRightHandSlots[_playerManager.playerInventoryManager.rightHandWeaponIndex];
                    
                    // Assign the network weapon id, so it switches for all connected clients
                    _playerManager.playerNetworkManager.currentRightHandWeaponID.Value = 
                        _playerManager.playerInventoryManager.weaponsInRightHandSlots[_playerManager.playerInventoryManager.rightHandWeaponIndex].itemID;
                    return;
                }
            }

            if (selectedWeapon == null && _playerManager.playerInventoryManager.rightHandWeaponIndex <= 2)
                SwitchRightWeapon();
        }
        #endregion
        
        #region Left Weapon
        public void LoadLeftWeapon()
        {
            if (_playerManager.playerInventoryManager.currentLeftHandWeapon != null)
            {
                // Remove the old weapon
                leftHandSlot.UnloadWeapon();
                
                // Bring in the new weapon
                leftHandWeaponModel = Instantiate(_playerManager.playerInventoryManager.currentLeftHandWeapon.weaponModel);
                leftHandSlot.LoadWeapon(leftHandWeaponModel);
                leftHandWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
                leftHandWeaponManager.SetWeaponDamage(_playerManager, _playerManager.playerInventoryManager.currentLeftHandWeapon);
            }
        }
        
        public void SwitchLeftWeapon()
        {
            
        }
        #endregion
        
    }
}
