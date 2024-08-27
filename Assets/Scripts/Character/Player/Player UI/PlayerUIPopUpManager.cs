using System.Collections;
using TMPro;
using UnityEngine;

namespace Character.Player.Player_UI
{
    public class PlayerUIPopUpManager : MonoBehaviour
    {
        [Header("YOU DIED PopUp")]
        [SerializeField] private GameObject youDiedPopUpGameObject;
        [SerializeField] private TextMeshProUGUI youDiedPopUpBackgroundText;
        [SerializeField] private TextMeshProUGUI youDiedPopUpText;
        [SerializeField] private CanvasGroup youDiedPopUpCanvasGroup;
        
        public void SendYouDiedPopUp()
        {
            // Activate post-processing effects
            
            youDiedPopUpGameObject.SetActive(true);
            youDiedPopUpBackgroundText.characterSpacing = 0;
            
            // Stretch out the pop-up
            StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 8f, 19f));
            
            // Fade in the pop-up
            StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 5f));
            
            // Wait, then fade out the pop-up
            StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiedPopUpCanvasGroup, 2f, 5f));
        }

        private static IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
        {
            if (duration > 0f)
            {
                text.characterSpacing = 0;
                var timer = 0f;
                
                yield return null;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20f));
                    yield return null;
                }
            }
        }
        
        private static IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
        {
            if (duration > 0f)
            {
                canvas.alpha = 0;
                var timer = 0f;
                
                yield return null;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
                    yield return null;
                }
            }
            
            canvas.alpha = 1;
            yield return null;
        }
        
        private static IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay)
        {
            if (duration > 0f)
            {
                while (delay > 0)
                {
                    delay -= Time.deltaTime;
                    yield return null;
                }
                
                canvas.alpha = 1;
                var timer = 0f;
                
                yield return null;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                    yield return null;
                }
            }
            
            canvas.alpha = 0;
            yield return null;
        }
    }
}
