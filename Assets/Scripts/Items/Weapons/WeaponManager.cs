using Character;
using Collider;
using UnityEngine;

namespace Items.Weapons
{
    public class WeaponManager : MonoBehaviour
    {
        public MeleeWeaponDamageCollider meleeDamageCollider;

        private void Awake() => meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();

        public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon)
        {
            meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;
            meleeDamageCollider.physicalDamage = weapon.physicalDamage;
            meleeDamageCollider.magicDamage = weapon.magicDamage;
            meleeDamageCollider.fireDamage = weapon.fireDamage;
            meleeDamageCollider.lightningDamage = weapon.lightningDamage;
            meleeDamageCollider.holyDamage = weapon.holyDamage;
            
            meleeDamageCollider.lightAttack_01_Modifier = weapon.lightAttack_01_Modifier;
        }
    }
}
