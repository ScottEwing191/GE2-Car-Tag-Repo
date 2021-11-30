using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CarTag.UI {
    public class ScreenFadeUI : MonoBehaviour {
        public const float DEFAULT_FADE_TIME = 1f;
        
        //--Serialized Fields
        [SerializeField] private Image screenFadeImage;
        //[SerializeField] private Color screenColour;
        [SerializeField] private float screenOpaqueTime = 0.5f;     // the time the screen will remain opaque before fading back in    

        //--Private
        private Coroutine fadeOutAndBackRoutine;

        private void Update() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.L)) {
                FadeOutAndBack();
            }
        }

        /// <summary>
        /// The will make the screen fade from transparent (alpha 0) to opaque (alpha 1) then back to transparent
        /// Only starts if the screen is not already fading
        /// </summary>
        public void FadeOutAndBack() {
            if (fadeOutAndBackRoutine == null) {
                fadeOutAndBackRoutine = StartCoroutine(FadeOutAndBackRoutine());
            }
            else {
                Debug.Log("Cannot start screen fade since screen fade enumerator is already running");
            }
        }
        private IEnumerator FadeOutAndBackRoutine() {
            yield return StartCoroutine(ScreenFadeRoutine(0, 1));
            yield return new WaitForSeconds(screenOpaqueTime);
            yield return StartCoroutine(ScreenFadeRoutine(1, 0));
            fadeOutAndBackRoutine = null;

        }

        public IEnumerator ScreenFadeRoutine(float fromAlpha, float toAlpha, float fadeTime = DEFAULT_FADE_TIME) {
            float time = 0;
            Color newColor = screenFadeImage.color;                 // new colour created, doesn't work if trying to modify the image colour alpha directly, don't remember why
            while (time <= fadeTime) {
                newColor.a = Mathf.Lerp(fromAlpha, toAlpha, time / fadeTime);
                screenFadeImage.color = newColor;
                time += Time.deltaTime;
                yield return null;
            }
            newColor.a = toAlpha;                                         // make sure alpha is exact orrect value
            screenFadeImage.color = newColor;

            yield return null;
        }

        //--Fades the given Canvas group from one alpha value to another
        public IEnumerator CanvasGroupFadeRoutine(float fromAlpha, float toAlpha, CanvasGroup group, float fadeTime = DEFAULT_FADE_TIME) {
            float time = 0;
            while (time <= fadeTime) {
                group.alpha = Mathf.Lerp(fromAlpha, toAlpha, time / fadeTime);
                time += Time.deltaTime;
                yield return null;
            }
            group.alpha = toAlpha;                                         // make sure alpha is exact correct value
            yield return null;
        }
    }
}
