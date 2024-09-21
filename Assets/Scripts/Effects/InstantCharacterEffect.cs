using Character;
using UnityEngine;

namespace Effects
{
    public class InstantCharacterEffect : ScriptableObject
    {
        [Header("Effect ID")]
        public int instantEffectID;
        
        public virtual void ProcessEffect(CharacterManager characterManager)
        {
            // Process the effect
        }
    }
}