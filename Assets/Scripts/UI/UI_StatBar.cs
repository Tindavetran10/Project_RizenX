using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UI_StatBar : MonoBehaviour
    {
        private Slider _slider;
        
        // Variables to scale bar size depending on the value of your stat
        // Secondary bar behind the main bar for polish effect (yellow bar that show how much an action/damage takes away from the main bar)

        protected virtual void Awake() => _slider = GetComponent<Slider>();

        public virtual void SetStat(float newValue) => _slider.value = newValue;

        public virtual void SetMaxStat(float maxValue)
        {
            _slider.maxValue = maxValue;
            _slider.value = maxValue;
        }
    }
}
