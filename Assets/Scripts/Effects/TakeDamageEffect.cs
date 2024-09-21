using Character;
using UnityEngine;
using World_Manager;

namespace Effects
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage Effect")]
    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage; // If the damage is caused by another character, attack will be stored here
        
        [Header("Damage")]
        public float physicalDamage = 0; // (In the future, we can add more types of damage like Standard, Strike, Slash and Pierce)
        public float magicalDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;
        
        [Header("Final Damage")]
        private int _finalDamageDealt = 0; // The damage the character will receive after All calculations have been made
        
        [Header("Poise")]
        public float poiseDamage = 0; // The amount of poise damage the character will receive
        public bool poiseIsBroken = false; // If a character's poise is broken, they will be "Stunned" and play the "Stunned" animation
        
        // (TODO)
        // Build Ups
        // Build Up Effect Amounts
        
        [Header("Animation")]
        public bool playDamageAnimation = true; // If true, the character will play the damage animation
        public bool manualSelectDamageAnimation = false; // If true, the character will play the damage animation
        public string damageAnimation; // The animation that will be played when the character receives damage
        
        [Header("Sound FX")]
        public bool willPlayDamageSfx = true; 
        public AudioClip elementalDamageSfx; // Used on top of regular damage SFX if there is elemental damage present

        [Header("Direction Damage Taken From")]
        public float angleHitFrom; // Used to determine the damage animation to play (Move backwards, to the left, to the right, etc.)
        public Vector3 contactPoint; // Used to determine where the blood FX instantiate
        
        public override void ProcessEffect(CharacterManager characterManager)
        {
            base.ProcessEffect(characterManager);
            
            // If the character is dead, do not process the effect
            if(characterManager.isDead.Value) return;
            
            // Check for "Invulnerable" status
            
            // Calculate damage
            CalculateDamage(characterManager);
            
            // Check which directional damage came from
            PlayDirectionalBaseDamageAnimation(characterManager);
            
            // Play damage animation
            
            // Check for build-ups (poison, bleed, etc.)
            
            // Play sound FX
            PlayDamageSFX(characterManager);
            
            // Play damage FX (blood, etc.)
            PlayDamageVFX(characterManager);
            
            // If character is AI, check for new target if character causing damage is present
        }
        
        private void CalculateDamage(CharacterManager characterManager)
        {
            if(!characterManager.IsOwner) return;
            
            if (characterCausingDamage != null)
            {
                // Check for damage modifiers and modify base damage (physical, magical, etc.)
                
            }
            
            // Check character for flat defense and subtract from damage
            
            // Check the character for armor absorption and subtract the percentage from the damage
            
            // Add all damage types together, and set the final damage dealt
            _finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicalDamage + fireDamage + lightningDamage + holyDamage);
            
            if(_finalDamageDealt <= 0) _finalDamageDealt = 1; // If the damage is 0 or less, set it to 1
            
            characterManager.characterNetworkManager.currentHealth.Value -= _finalDamageDealt; 
            
            // Calculate poise damage to determine if the character will be stunned
        }

        private void PlayDamageVFX(CharacterManager characterManager)
        {
            // if we have fire damage, play fire VFX
            // if we have lightning damage, play lightning VFX etc.
            
            characterManager.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
        }
        
        private void PlayDamageSFX(CharacterManager characterManager)
        {
            // Play the damage SFX
            var physicalDamageSfx = WorldSoundFxManager.ChooseRandomSFXFromArray(
                WorldSoundFxManager.instance.physicalDamageSfx);
            
            characterManager.characterSoundFxManager.PlaySoundFX(physicalDamageSfx);
        }
        
        private void PlayDirectionalBaseDamageAnimation(CharacterManager characterManager)
        {
            if(!characterManager.IsOwner) return;
            
            poiseIsBroken = true; // For now, we will always break poise
            
            // Play the damage animation based on the angle hit from
            if (angleHitFrom >= 145 && angleHitFrom <= 180)
                // Play front damage animation
                damageAnimation = characterManager.characterAnimatorManager.GetRandomAnimationFromList(characterManager.characterAnimatorManager.ForwardMediumDamage);
            else if (angleHitFrom <= -145 && angleHitFrom >= -180)
                // Play front damage animation 
                damageAnimation = characterManager.characterAnimatorManager.GetRandomAnimationFromList(characterManager.characterAnimatorManager.ForwardMediumDamage);
            else if (angleHitFrom >= -45 && angleHitFrom <= 45)
                // Play back damage animation
                damageAnimation = characterManager.characterAnimatorManager.GetRandomAnimationFromList(characterManager.characterAnimatorManager.BackwardMediumDamage);
            else if (angleHitFrom >= -144 && angleHitFrom <= -45)
                // Play left damage animation
                damageAnimation = characterManager.characterAnimatorManager.GetRandomAnimationFromList(characterManager.characterAnimatorManager.LeftMediumDamage);
            else if (angleHitFrom >= 45 && angleHitFrom <= 144)
                // Play right damage animation
                damageAnimation = characterManager.characterAnimatorManager.GetRandomAnimationFromList(characterManager.characterAnimatorManager.RightMediumDamage);

            // Play the damage animation based on the calculated angle
            /*if (angleHitFrom >= -45 && angleHitFrom <= 45)
            {
                // Play front damage animation
                damageAnimation = characterManager.characterAnimatorManager.GetRandomAnimationFromList(characterManager.characterAnimatorManager.ForwardMediumDamage);
            }
            else if (angleHitFrom > 45 && angleHitFrom <= 135)
            {
                // Play left damage animation
                damageAnimation = characterManager.characterAnimatorManager.GetRandomAnimationFromList(characterManager.characterAnimatorManager.LeftMediumDamage);
            }
            else if (angleHitFrom < -45 && angleHitFrom >= -135)
            {
                // Play right damage animation
                damageAnimation = characterManager.characterAnimatorManager.GetRandomAnimationFromList(characterManager.characterAnimatorManager.RightMediumDamage);
            }
            else
            {
                // Play back damage animation
                damageAnimation = characterManager.characterAnimatorManager.GetRandomAnimationFromList(characterManager.characterAnimatorManager.BackwardMediumDamage);
            }*/
            
            if (poiseIsBroken)
            {
                characterManager.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;
                characterManager.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
            }
        }
    }
}