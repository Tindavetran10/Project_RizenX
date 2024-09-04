using Items.Weapons;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "New Melee Weapon", menuName = "Items/Weapons/Melee Weapon")]
    public class MeleeWeaponItem : WeaponItem
    {
        // Weapon "Deflection" (If the weapon bounces off another weapon when it is guarded against) 
        // Can be Buffed
    }
}