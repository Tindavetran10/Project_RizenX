using UnityEngine;

namespace Items
{
    public class WeaponItem : Item
    {
        // Animator Controller override (Change attack animations based on weapon you are currently using)
        [Header("Weapon Model")]
        public GameObject weaponModel;
        
        [Header("Weapon Requirements")]
        public int strengthRequirement = 0;
        public int dexterityRequirement = 0;
        public int intelligenceRequirement = 0;
        public int faithRequirement = 0;
        
        [Header("Weapon Base Damage")]
        public int physicalDamage = 0;
        public int magicDamage = 0;
        public int fireDamage = 0;
        public int lightningDamage = 0;
        public int holyDamage = 0;
        
        // Weapon Guard Absorption (Blocking Power)
        
        [Header("Weapon Poise")]
        public float poiseDamage = 10;
        
        // Offensive Poise Bonus When Attacking
        
        // Weapon Modifiers
        // Light Attack Modifiers
        // Heavy Attack Modifiers
        // Critical Attack Modifiers
        
        [Header("Stamina Costs")]
        public int baseStaminaCost = 20;
        
        // Running attack stamina cost modifier
        // Light attack stamina cost modifier
        // Heavy attack stamina cost modifier
        
        // Item Based Actions
        
        // Ash of War
        
        // Blocking Weapon
    }
}