using Effects;
using UnityEngine;

namespace Character
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        // Process instant effects (take damage, heal, etc.)
        
        // Process timed effects (poison, burn, etc.)
        
        // Process static effects (adding/removing buffs from talisman, etc.)
        
        private CharacterManager _characterManager;

        protected virtual void Awake() => _characterManager = GetComponent<CharacterManager>();

        protected virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            // Take in an effect, and apply it to the character   
            
            // Process it
            effect.ProcessEffect(_characterManager);
        }
    }
}