using Character;
using Effects;
using UnityEngine;
using World_Manager;

namespace Collider
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage; // When calculating damage, this is used to check for attacker damage modifiers, effects, etc.
    
        [Header("Weapon Attack Modifiers")]
        public float lightAttack_01_Modifier = 2.0f;

        protected override void Awake()
        {
            base.Awake();
            
            if(_damageCollider == null)
                _damageCollider = GetComponent<UnityEngine.Collider>();
            
            _damageCollider.enabled = false; // Melee weapon colliders are disabled by default, only enabled during attack animations
        }
        
        protected override void OnTriggerEnter(UnityEngine.Collider other)
        {
            var damageTarget = other.GetComponentInParent<CharacterManager>();
            
            // If you want to search on both the damageable character colliders & the character controller collider,
            // check for null here and do the following  
            
            /*if(damageTarget == null)
                damageTarget = other.GetComponent<CharacterManager>();*/
            
            
            if (damageTarget != null)
            {
                if(damageTarget == characterCausingDamage) return; // Don't damage yourself
                
                ContactPoint = other.gameObject.GetComponent<UnityEngine.Collider>().ClosestPointOnBounds(transform.position);
            
                // check if we can damage this target based on friendly fire settings
            
                // check if target is blocking
            
                // check if target is invulnerable
            
                // damage the target
                DamageTarget(damageTarget);
            }
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            // We don't want to damage the same target multiple times,
            // So we add them to a list that checks before applying damage 

            if (CharactersDamaged.Contains(damageTarget)) return;

            CharactersDamaged.Add(damageTarget);

            var damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicalDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;

            damageEffect.contactPoint = ContactPoint;
            damageEffect.angleHitFrom =
                Vector3.SignedAngle(characterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

            switch (characterCausingDamage.characterCombatManager.currentAttackType)
            {
                case AttackType.LightAttack01:
                    ApplyAttackDamageModifiers(lightAttack_01_Modifier, damageEffect);
                    break;
                case AttackType.HeavyAttack:
                    break;
                case AttackType.SpecialAttack:
                    break;
                case AttackType.Block:
                    break;
                case AttackType.Parry:
                    break;
                case AttackType.NO_ATTACK:
                    break;
            }

            //damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);

            if (characterCausingDamage.IsOwner)
            {
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                    damageTarget.NetworkObjectId,
                    characterCausingDamage.NetworkObjectId,
                    damageEffect.physicalDamage,
                    damageEffect.magicalDamage,
                    damageEffect.fireDamage,
                    damageEffect.lightningDamage,
                    damageEffect.holyDamage,
                    damageEffect.poiseDamage,
                    damageEffect.angleHitFrom,
                    damageEffect.contactPoint.x,
                    damageEffect.contactPoint.y,
                    damageEffect.contactPoint.z);
            }
        }
        
        private static void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damageEffect)
        {
            damageEffect.physicalDamage *= modifier;
            damageEffect.magicalDamage *= modifier;
            damageEffect.fireDamage *= modifier;
            damageEffect.lightningDamage *= modifier;
            damageEffect.holyDamage *= modifier;
            damageEffect.poiseDamage *= modifier;
        }
    }
}


