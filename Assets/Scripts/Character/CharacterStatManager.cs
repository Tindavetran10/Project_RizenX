using UnityEngine;

namespace Character
{
    public class CharacterStatManager : MonoBehaviour
    {
        private CharacterManager _characterManager;

        [Header("Stamina Regeneration")] 
        [SerializeField] private float staminaRegenerationAmount = 2f;
        private float _staminaRegenerationTimer;
        private float _staminaTickTimer;
        [SerializeField] private float staminaRegenerationDelay = 3f;

        protected virtual void Awake() => _characterManager = GetComponent<CharacterManager>();

        protected virtual void Start() {}

        public int CalculateHealthBasedOnVitalityLevel(int vitality)
        {
            // Calculate health based on level
            float health = vitality * 15;
            return Mathf.RoundToInt(health);
        }
        
        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            // Calculate stamina based on level
            float stamina = endurance * 10;
            return Mathf.RoundToInt(stamina);
        }

        public virtual void RegenerateStamina()
        {
            if (!_characterManager.IsOwner) return;

            if (_characterManager.characterNetworkManager.isSprinting.Value) return;

            if (_characterManager.isPerformingAction) return;

            _staminaRegenerationTimer += Time.deltaTime;

            if (_staminaRegenerationTimer >= staminaRegenerationDelay)
            {
                if (_characterManager.characterNetworkManager.currentStamina.Value <
                    _characterManager.characterNetworkManager.maxStamina.Value)
                {
                    _staminaTickTimer += Time.deltaTime;

                    if (_staminaTickTimer >= 0.1f)
                    {
                        _staminaTickTimer = 0f;
                        _characterManager.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                    }
                }
            }
        }
        
        public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
        {
            // We only want to reset the regeneration timer if the action used stamina.
            // We don't want to reset the timer if we are already regenerating stamina
            if(currentStaminaAmount < previousStaminaAmount) _staminaRegenerationTimer = 0f;
        }
    }
}