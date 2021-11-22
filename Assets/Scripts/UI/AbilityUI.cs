using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.UI {
    public class AbilityUI {
        private AbilityUIElements abilityUIElements;
        
        public AbilityUI(AbilityUIElements abilityUIElements) {
            this.abilityUIElements = abilityUIElements;
            SetDefaultValues();
        }

        private void SetDefaultValues() {
            SetTimerFilledImage(1);
            SetUsesLeftIndicators(4);
            SetSelectedIcon(0);
            SetSelectedIndicator(0);
        }
        

        public void SetUsesLeftIndicators(int usesLeft) {
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

        public void SetTimerFilledImage(float fillValue) {
            abilityUIElements.TimerFilledImage.fillAmount = fillValue;
        }

        
    }
}
