using Character;
using Collider;
using UnityEngine;

public class MeleeWeaponDamageCollider : DamageCollider
{
    [Header("Attacking Character")]
    public CharacterManager characterCausingDamage; // When calculating damage, this is used to check for attacker damage modifiers, effects, etc.
    
}


