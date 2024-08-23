using Character.Player.Player_UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UI_StatBar : MonoBehaviour
    {
        private Slider _slider;
        private RectTransform _rectTransform;
        
        // Variables to scale bar size depending on the value of your stat
        [Header("Bar Options")]
        [SerializeField] protected bool scaleBarLengthWithStats = true;
        [SerializeField] protected float widthLengthMultiplier = 1.0f;
        
        // Secondary bar behind the main bar for polish effect
        // (yellow bar that shows how much an action/damage takes away from the main bar)

        protected virtual void Awake()
        {
            _slider = GetComponent<Slider>();
            _rectTransform = GetComponent<RectTransform>();
        }

        public virtual void SetStat(float newValue) => _slider.value = newValue;

        public virtual void SetMaxStat(float maxValue)
        {
            _slider.maxValue = maxValue;
            _slider.value = maxValue;
            
            if (scaleBarLengthWithStats)
            {
                // Scale the transform of this object
                _rectTransform.sizeDelta = new Vector2(maxValue * widthLengthMultiplier, _rectTransform.sizeDelta.y);
                
                // Reset the position of the bars on their layout group settings
                PlayerUIManager.Instance.playerUIHudManager.RefreshUI();
            }
        }
    }
}
