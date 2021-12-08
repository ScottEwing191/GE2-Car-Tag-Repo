using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.UI;

namespace CarTag.MainMenu {
    public class MainMenuUIManager : MonoBehaviour {
        [SerializeField] private ScreenFadeUI screenFade;
        [SerializeField] private CanvasGroup mainMenu;
        [SerializeField] private CanvasGroup controls;
        [SerializeField] private CanvasGroup rules;


        private void Awake() {
            Time.timeScale = 1;
            controls.gameObject.SetActive(false);
            rules.gameObject.SetActive(false);

        }
        public void ExitGame() {
            Application.Quit();
        }


        public void ControlsButton() {
            StartCoroutine(SwitchCanvasGroup(mainMenu, controls));
        }
        public void RulesButton() {
            StartCoroutine(SwitchCanvasGroup(mainMenu, rules));

        }
        public void BackFromControls() {
            StartCoroutine(SwitchCanvasGroup(controls, mainMenu));

        }
        public void BackFromRules() {
            StartCoroutine(SwitchCanvasGroup(rules, mainMenu));

        }
        public IEnumerator SwitchCanvasGroup(CanvasGroup fromGroup, CanvasGroup toGroup) {
            toGroup.gameObject.SetActive(true);
            StartCoroutine(screenFade.CanvasGroupFadeRoutine(0, 1, toGroup));
            yield return StartCoroutine(screenFade.CanvasGroupFadeRoutine(1, 0, fromGroup));
            fromGroup.gameObject.SetActive(false);
        }

    }
}
