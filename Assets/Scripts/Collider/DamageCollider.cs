using System.Collections.Generic;
using Character;
using UnityEngine;
using World_Manager;

namespace Collider
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Collider")] 
        protected UnityEngine.Collider _damageCollider;
    
        [Header("Damage")]
        public float physicalDamage = 0; // (In the future, we can add more types of damage like Standard, Strike, Slash and Pierce)
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;
    
        [Header("Contact Point")]
        private Vector3 _contactPoint; // Used to determine where the blood FX instantiate

        [Header("Characters Damaged")]
        private readonly List<CharacterManager> _charactersDamaged = new();
    
        private void OnTriggerEnter(UnityEngine.Collider other)
        {
            var damageTarget = other.GetComponentInParent<CharacterManager>();
            
            // If you want to search on both the damageable character colliders & the character controller collider,
            // check for null here and do the following  
            
            /*if(damageTarget == null)
                damageTarget = other.GetComponent<CharacterManager>();*/
            
            if (damageTarget != null)
            {
                _contactPoint = other.gameObject.GetComponent<UnityEngine.Collider>().ClosestPointOnBounds(transform.position);
            
                // check if we can damage this target based on friendly fire settings
            
                // check if target is blocking
            
                // check if target is invulnerable
            
                // damage the target
                DamageTarget(damageTarget);
                Debug.Log(other);
            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
        {
            // We don't want to damage the same target multiple times,
            // So we add them to a list that checks before applying damage 

            if (_charactersDamaged.Contains(damageTarget)) return;

            _charactersDamaged.Add(damageTarget);

            var damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicalDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;

            damageEffect.contactPoint = _contactPoint;

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }

        public virtual void EnableDamageCollider() => _damageCollider.enabled = true;

        public virtual void DisableDamageCollider()
        {
            _damageCollider.enabled = false;
            // We reset the characters that have been hit when we reset the collider, so they may be hit again
            _charactersDamaged.Clear();
        }
    }
}
