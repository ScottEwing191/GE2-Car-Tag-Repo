using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CarTag.UI;

namespace CarTag.MainMenu
{
    public class SelectPlayersButtons : MonoBehaviour
    {
        [SerializeField] private PlayersPlaying playersPlaying;
        [SerializeField] private ScreenFadeUI screenFadeUI;
        [SerializeField] private MainMenuUIManager mainMenuUIManager;
        private bool selected = false;                      // stops the player from double clicking buttons

        

        public void SelectTwoPlayers() {
            if (!selected) {
                selected = true;
                playersPlaying.NumberOfPlayers = 2;
                //mainMenuUIManager.GoToControllerSelect();
                StartCoroutine(LoadNextScene());
            }
        }
        public void SelectThreePlayers() {
            if (!selected) {
                selected = true;
                playersPlaying.NumberOfPlayers = 3;
                //mainMenuUIManager.GoToControllerSelect();
                StartCoroutine(LoadNextScene());
            }
        }
        public void SelectFourPlayers() {
            if (!selected) {
                selected = true;
                playersPlaying.NumberOfPlayers = 4;
                //mainMenuUIManager.GoToControllerSelect();
                StartCoroutine(LoadNextScene());
            }
        }

        public IEnumerator LoadNextScene() {
            yield return StartCoroutine(screenFadeUI.ScreenFadeRoutine(0, 1));
            SceneManager.LoadScene("Level 01 City RCC");
        }
    }
}
