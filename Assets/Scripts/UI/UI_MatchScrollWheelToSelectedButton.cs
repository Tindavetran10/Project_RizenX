using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UI_MatchScrollWheelToSelectedButton : MonoBehaviour
    {
        [SerializeField] private GameObject currentSelected;
        [SerializeField] private GameObject previouslySelected;
        [SerializeField] private RectTransform currentSelectedTransform;
        
        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private ScrollRect scrollRect;

        private void Update()
        {
            currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected != null)
            {
                if (currentSelected != previouslySelected)
                {
                    previouslySelected = currentSelected;
                    currentSelectedTransform = currentSelected.GetComponent<RectTransform>();
                    SnapTo(currentSelectedTransform);
                }
            }
        }

        private void SnapTo(RectTransform target)
        {
            Canvas.ForceUpdateCanvases();
            
            Vector2 newPosition = (Vector2) scrollRect.transform.InverseTransformPoint(contentPanel.position) - 
                                  (Vector2) scrollRect.transform.InverseTransformPoint(target.position);
            
            // We only want to lock the position on the Y axis (up and down)
            newPosition.x = 0;
            
            contentPanel.anchoredPosition = newPosition;
        }
        
    }
}
