using UI;
using UnityEngine;

namespace Character.Player.Player_UI
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [SerializeField] private UI_StatBar staminaBar;
        [SerializeField] private UI_StatBar healthBar;
        
        public void RefreshUI()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);
            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
        }
        
        public void SetNewHealthValue(int oldValue, int newValue) => 
            healthBar.SetStat(newValue);

        public void SetMaxHealthValue(int maxHealth) => 
            healthBar.SetMaxStat(maxHealth);
        
        public void SetNewStaminaValue(float oldValue, float newValue) => 
            staminaBar.SetStat(Mathf.RoundToInt(newValue));

        public void SetMaxStaminaValue(int maxStamina) => 
            staminaBar.SetMaxStat(maxStamina);
    }
}
