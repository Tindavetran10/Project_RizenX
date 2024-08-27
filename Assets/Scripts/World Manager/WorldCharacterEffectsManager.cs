using System.Collections.Generic;
using Effects;
using UnityEngine;

namespace World_Manager
{
    public class WorldCharacterEffectsManager : MonoBehaviour
    {
        public static WorldCharacterEffectsManager instance;
    
        [Header("Damage")]
        public TakeDamageEffect takeDamageEffect;
        
        [SerializeField] private List<InstantCharacterEffect> instantEffects;
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else Destroy(gameObject);
        
            GenerateEffectIDs();
        }

        private void GenerateEffectIDs()
        {
            for(var i = 0; i < instantEffects.Count; i++) 
                instantEffects[i].instantEffectID = i;
        }
    }
}
