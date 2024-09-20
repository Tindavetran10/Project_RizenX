using Effects;
using UnityEngine;
using World_Manager;

namespace Character
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        // Process instant effects (take damage, heal, etc.)
        
        // Process timed effects (poison, burn, etc.)
        
        // Process static effects (adding/removing buffs from talisman, etc.)
        
        private CharacterManager _characterManager;

        [Header("VFX")] 
        [SerializeField] private GameObject bloodSplatterVFX;
        
        protected virtual void Awake() => _characterManager = GetComponent<CharacterManager>();

        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            // Take in an effect, and apply it to the character   
            
            // Process it
            effect.ProcessEffect(_characterManager);
        }
        
        public void PlayBloodSplatterVFX(Vector3 contactPoint)
        {
            // if we have a custom blood splatter VFX on this model, play it
            if(bloodSplatterVFX != null)
            {
                GameObject bloodSplatter = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
            // else, use the generic (default) blood splatter VFX
            else
            {
                GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
        }
    }
}