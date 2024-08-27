using Effects;
using UnityEngine;

namespace Character.Player
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        [Header("Debug Delete Later")]
        [SerializeField] private InstantCharacterEffect effectToTest;
        [SerializeField] private bool processEffect;
        
        private void Update()
        {
            if (processEffect)  
            {
                processEffect = false;
                
                // We instantiate the effect to test it, so it won't affect the original
                var effect = Instantiate(effectToTest);
                ProcessInstantEffect(effect);
            }
        }
    }
}