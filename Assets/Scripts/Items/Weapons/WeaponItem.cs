using UnityEngine;
using Weapon_Actions;

namespace Items.Weapons
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
        [Header("Attack Modifiers")]
        // Light Attack Modifiers
        public float lightAttack_01_Modifier = 1.1f;
        // Heavy Attack Modifiers
        // Critical Attack Modifiers
        
        [Header("Stamina Costs Modifiers")]
        public int baseStaminaCost = 20;
        // Light attack stamina cost modifier
        public float lightAttackStaminaCostMultiplier = 0.9f;
        // Running attack stamina cost modifier
        // Heavy attack stamina cost modifier
        
        [Header("Actions")]
        public WeaponItemActions oh_rb_Action;
        
        // Item Based Actions
        
        // Ash of War
        
        // Blocking Weapon
    }
}