using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.UI {
    public class AbilityUI :MonoBehaviour {
        //-- Private
        private AbilityUIElements abilityUIElements;
        private Coroutine abilityCooldownTimerUIRoutine;


        public void InitialSetup(AbilityUIElements abilityUIElements, int initialUsesLeft) {
            this.abilityUIElements = abilityUIElements;
            //SetDefaultValues();
            SetTimerFilledImage(1);
            SetUsesLeftIndicators(initialUsesLeft);
            SetSelectedIcon(0);
            SetSelectedIndicator(0);
        }

        private void SetDefaultValues() {
            SetTimerFilledImage(1);
            SetUsesLeftIndicators(4);
            SetSelectedIcon(0);
            SetSelectedIndicator(0);
        }
        

        private void SetUsesLeftIndicators(int usesLeft) {
            //--the UI currently only has the capacity for show between 0 - 4 uses left so if there are more than 4 uses left it wil just show the 4 indicators
            usesLeft = Mathf.Clamp(usesLeft, 0, 4);
            for (int i = 0; i < 4; i++) {
                abilityUIElements.UsesLeftIndicators[i].SetActive(false);
            }
            int usesLeftAsIndex = usesLeft - 1;
            for (int i = 0; i < usesLeft; i++) {
                abilityUIElements.UsesLeftIndicators[i].SetActive(true);
            }

        }
        private void SetSelectedIcon(int selectedIconIndex) {
            for (int i = 0; i < abilityUIElements.SelectedIcons.Count; i++) {
                if (i == selectedIconIndex)
                    abilityUIElements.SelectedIcons[i].SetActive(true);
                else
                    abilityUIElements.SelectedIcons[i].SetActive(false);
            }
        }
        private void SetSelectedIndicator(int selectedIndicatorIndex) {
            for (int i = 0; i < abilityUIElements.AbilitiesToSelect.Count; i++) {
                if (i == selectedIndicatorIndex)
                    abilityUIElements.AbilitiesToSelect[i].SelectedIndicator.SetActive(true);
                else
                    abilityUIElements.AbilitiesToSelect[i].SelectedIndicator.SetActive(false);
            }
        }

        private void SetTimerFilledImage(float fillValue) {
            abilityUIElements.TimerFilledImage.fillAmount = fillValue;
        }


        //--Called each time an ability is used. Will update the Indicaters which show how many uses of an ability are left
        //--Starts coroutine to display time until an ability can be used again
        internal void UpdateAbilityUIOnUse(float time, int usesLeft) {
            abilityCooldownTimerUIRoutine = StartCoroutine(AbilityCooldownTimerUIRoutine(time));
            SetUsesLeftIndicators(usesLeft);
        }
        private IEnumerator AbilityCooldownTimerUIRoutine(float cooldownTime) {
            float timeLeft = cooldownTime;
            while (timeLeft >= 0) {
                SetTimerFilledImage(Mathf.Lerp(1, 0, timeLeft / cooldownTime));
                yield return new WaitForSeconds(0.1f);
                timeLeft -= 0.1f;
            }
        }

        public void ResetAbilityUI(int usesLeft) {              // uses left will be the max number of uses for the current ability and may differ if the player
            if (abilityCooldownTimerUIRoutine != null) {        // depending on if the player is the Runner or the Chaser
                StopCoroutine(abilityCooldownTimerUIRoutine);
                abilityCooldownTimerUIRoutine = null;
            }
            SetUsesLeftIndicators(usesLeft);
            SetTimerFilledImage(1);
        }
    }
}
