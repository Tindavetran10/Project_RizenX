using System.Collections.Generic;
using Character;
using Effects;
using UnityEngine;
using World_Manager;

public class DamageCollider : MonoBehaviour
{
    [Header("Damage")]
    public float physicalDamage = 0; // (In the future, we can add more types of damage like Standard, Strike, Slash and Pierce)
    public float magicalDamage = 0;
    public float fireDamage = 0;
    public float lightningDamage = 0;
    public float holyDamage = 0;
    
    [Header("Contact Point")]
    private Vector3 _contactPoint; // Used to determine where the blood FX instantiate

    [Header("Characters Damaged")]
    private readonly List<CharacterManager> _charactersDamaged = new();
    
    private void OnTriggerEnter(Collider other)
    {
        var damageTarget = other.GetComponent<CharacterManager>();
        
        if (damageTarget != null)
        {
            _contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            
            // check if we can damage this target based on friendly fire settings
            
            // check if target is blocking
            
            // check if target is invulnerable
            
            // damage the target
            DamageTarget(damageTarget);
        }
    }

    protected virtual void DamageTarget(CharacterManager damageTarget)
    {
        // We don't want to damage the same target multiple times
        // So we add them to a list that checks before applying damage 

        if (_charactersDamaged.Contains(damageTarget)) return;

        _charactersDamaged.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);

        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicalDamage = magicalDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.lightningDamage = lightningDamage;
        damageEffect.holyDamage = holyDamage;

        damageEffect.contactPoint = _contactPoint;

        damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
    }
}
