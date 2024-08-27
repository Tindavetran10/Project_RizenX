using Character;
using UnityEngine;

namespace Effects
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage Effect")]
    public class TakeStaminaDamageEffect : InstantCharacterEffect
    {
        public float staminaDamage;
        public override void ProcessEffect(CharacterManager characterManager)
        {
            CalculateStaminaDamage(characterManager);
        }
        
        private void CalculateStaminaDamage(CharacterManager characterManager)
        {
            // Compare the base stamina damage against other player effect/modifiers
            // Change the value before subtracting/adding it to the player's stamina
            // Play sound effect or visual effect

            if (characterManager.IsOwner) 
                characterManager.characterNetworkManager.currentStamina.Value -= staminaDamage;
        }
    }
}