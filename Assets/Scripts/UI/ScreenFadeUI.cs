using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CarTag.UI {
    public class ScreenFadeUI : MonoBehaviour {
        //--Serialized Fields
        [SerializeField] private Image screenFadeImage;
        //[SerializeField] private Color screenColour;
        [SerializeField] private float screenFadeTime = 2;          // the time taken for the screen from opaque to transparent
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

        private IEnumerator ScreenFadeRoutine(float fromAlpha, float toAlpha) {
            float time = 0;
            Color newColor = screenFadeImage.color;                 // new colour created, doesn't work if trying to modify the image colour alpha directly, don't remember why
            while (time <= screenFadeTime) {
                newColor.a = Mathf.Lerp(fromAlpha, toAlpha, time / screenFadeTime);
                screenFadeImage.color = newColor;
                time += Time.deltaTime;
                yield return null;
            }
            newColor.a = toAlpha;                                         // make sure alpha is exact orrect value
            screenFadeImage.color = newColor;

            yield return null;
        }
    }
}
