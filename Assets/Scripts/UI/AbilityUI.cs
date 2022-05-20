using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.UI {
    public class AbilityUI : MonoBehaviour {
        [SerializeField] private AbilityUIElements runnerAbilityUIElements;
        [SerializeField] private AbilityUIElements chaserAbilityUIElements;
        public AbilityActiveTimerUI AbilityActiveTimerUI { get; set; }
        public PlayerUIController PlayerUIController { get; private set; }

        //-- Private
        private AbilityUIElements activeAbilityUIElements;
        private Coroutine abilityCooldownTimerUIRoutine;

        private void Awake() {
            AbilityActiveTimerUI = GetComponentInChildren<AbilityActiveTimerUI>();
            PlayerUIController = GetComponentInParent<PlayerUIController>();

        }

        private void Start() {
            PlayerUIController.thisPlayer.PlayerEvents.AbilityEvents.currentAbilityChanged += ChangeAbilityUI;
        }

        private void OnDestroy() {
            PlayerUIController.thisPlayer.PlayerEvents.AbilityEvents.currentAbilityChanged -= ChangeAbilityUI;
        }

        public void InitialSetup(int initialUsesLeft, bool isRunner) {
            SetActiveAbilityElements(isRunner);
            //SetDefaultValues();
            SetTimerFilledImage(1);
            SetUsesLeftIndicators(initialUsesLeft);
            SetUsesLeftText(initialUsesLeft);
            SetSelectedText(0);
            SetSelectedIcon(0);
            SetAbilitySelectedIndicator(0);
        }

        

        //--Called each time an ability is used. Will update the Indicaters which show how many uses of an ability are left
        //--Starts coroutine to display time until an ability can be used again
        internal void UpdateAbilityUIOnUse(float time, int usesLeft) {
            abilityCooldownTimerUIRoutine = StartCoroutine(AbilityCooldownTimerUIRoutine(time));
            //--Set USes Left Text

            SetUsesLeftIndicators(usesLeft);
            SetUsesLeftText(usesLeft);
        }
        private IEnumerator AbilityCooldownTimerUIRoutine(float cooldownTime) {
            float timeLeft = cooldownTime;
            while (timeLeft >= 0) {
                SetTimerFilledImage(Mathf.Lerp(1, 0, timeLeft / cooldownTime));
                yield return new WaitForSeconds(0.1f);
                timeLeft -= 0.1f;
            }
        }

        //Changes the UI to Show the currently active Ability
        internal void ChangeAbilityUI(int usesLeft, int selectedIndex) {
            SetUsesLeftIndicators(usesLeft);
            SetAbilitySelectedIndicator(selectedIndex);
            SetSelectedIcon(selectedIndex);
            SetUsesLeftText(usesLeft);
            SetSelectedText(selectedIndex);
        }


        public void ResetAbilityUI(int usesLeft, bool isRunner, int selectedAbilityIndex) {              // uses left will be the max number of uses for the current ability and may differ if the player
            if (abilityCooldownTimerUIRoutine != null) {        // depending on if the player is the Runner or the Chaser
                StopCoroutine(abilityCooldownTimerUIRoutine);
                abilityCooldownTimerUIRoutine = null;
            }
            SetActiveAbilityElements(isRunner);
            SetSelectedIcon(selectedAbilityIndex);
            SetAbilitySelectedIndicator(selectedAbilityIndex);
            SetUsesLeftIndicators(usesLeft);
            SetTimerFilledImage(1);
            SetUsesLeftText(usesLeft);
            SetSelectedText(selectedAbilityIndex);
        }

        private void SetActiveAbilityElements(bool isRunner) {
            if (isRunner) {
                chaserAbilityUIElements.AbilityUIElementsObjects.SetActive(false);
                runnerAbilityUIElements.AbilityUIElementsObjects.SetActive(true);
                activeAbilityUIElements = runnerAbilityUIElements;
            }
            else {
                chaserAbilityUIElements.AbilityUIElementsObjects.SetActive(true);
                runnerAbilityUIElements.AbilityUIElementsObjects.SetActive(false);
                activeAbilityUIElements = chaserAbilityUIElements;
            }
        }

        private void SetUsesLeftIndicators(int usesLeft) {
            //--the UI currently only has the capacity for show between 0 - 4 uses left so if there are more than 4 uses left it wil just show the 4 indicators
            usesLeft = Mathf.Clamp(usesLeft, 0, 4);
            for (int i = 0; i < 4; i++) {
                activeAbilityUIElements.UsesLeftIndicators[i].SetActive(false);
            }
            int usesLeftAsIndex = usesLeft - 1;
            for (int i = 0; i < usesLeft; i++) {
                activeAbilityUIElements.UsesLeftIndicators[i].SetActive(true);
            }

        }
        private void SetSelectedIcon(int selectedIconIndex) {
            for (int i = 0; i < activeAbilityUIElements.SelectedIcons.Count; i++) {
                if (i == selectedIconIndex)
                    activeAbilityUIElements.SelectedIcons[i].SetActive(true);
                else
                    activeAbilityUIElements.SelectedIcons[i].SetActive(false);
            }
        }
        private void SetAbilitySelectedIndicator(int selectedIndicatorIndex) {
            for (int i = 0; i < activeAbilityUIElements.AbilitiesToSelect.Count; i++) {
                if (i == selectedIndicatorIndex)
                    activeAbilityUIElements.AbilitiesToSelect[i].SelectedIndicator.SetActive(true);
                else
                    activeAbilityUIElements.AbilitiesToSelect[i].SelectedIndicator.SetActive(false);
            }
        }

        private void SetTimerFilledImage(float fillValue) {
            activeAbilityUIElements.TimerFilledImage.fillAmount = fillValue;
        }

        private void SetSelectedText(int selectedIndex) {
            switch (selectedIndex) {
                case 0: {
                        activeAbilityUIElements.SelectedAbilityText.SetText("Boxes");
                        break;

                    }
                case 1: {
                        activeAbilityUIElements.SelectedAbilityText.SetText("Rockets");
                        break;

                    }
                case 2: {
                        activeAbilityUIElements.SelectedAbilityText.SetText("Slow Mo");
                        break;

                    }
                default: 
                    break;
            }
        }

        private void SetUsesLeftText(int usesLeft) {
            if (usesLeft == -1) {
                print("");
            }
            activeAbilityUIElements.UsesLeftText.SetText(usesLeft.ToString());
        }


    }
}
