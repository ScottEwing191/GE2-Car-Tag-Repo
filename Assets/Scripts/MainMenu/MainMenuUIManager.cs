using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.UI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace CarTag.MainMenu {
    public class MainMenuUIManager : MonoBehaviour {
        [SerializeField] private ScreenFadeUI screenFade;
        [Header("Canves Groups")]
        [SerializeField] private CanvasGroup mainMenu;
        [SerializeField] private CanvasGroup controls;
        [SerializeField] private CanvasGroup rules;
        [SerializeField] private CanvasGroup setControls;

        [Header("Buttons")]
        [SerializeField] private Button defaultButton;
        [SerializeField] private Button controlsBackBtn;
        [SerializeField] private Button rulesBackBtn;
        [SerializeField] private Button playButton;

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
            controlsBackBtn.Select();
        }
        public void RulesButton() {
            StartCoroutine(SwitchCanvasGroup(mainMenu, rules));
            rulesBackBtn.Select();
        }
        public void BackFromControls() {
            StartCoroutine(SwitchCanvasGroup(controls, mainMenu));
            defaultButton.Select();

        }
        public void BackFromRules() {
            StartCoroutine(SwitchCanvasGroup(rules, mainMenu));
            defaultButton.Select();
        }

        public void GoToControllerSelect() {
            StartCoroutine(SwitchCanvasGroup(mainMenu, setControls));
            playButton.Select();
        }
        public IEnumerator SwitchCanvasGroup(CanvasGroup fromGroup, CanvasGroup toGroup) {
            toGroup.gameObject.SetActive(true);
            StartCoroutine(screenFade.CanvasGroupFadeRoutine(0, 1, toGroup));
            yield return StartCoroutine(screenFade.CanvasGroupFadeRoutine(1, 0, fromGroup));
            fromGroup.gameObject.SetActive(false);
        }

        public void StartGame() {
            StartCoroutine(LoadNextScene());

        }
        public IEnumerator LoadNextScene() {
            yield return StartCoroutine(screenFade.ScreenFadeRoutine(0, 1));
            SceneManager.LoadScene("Level 01 City RCC");
        }

    }
}
