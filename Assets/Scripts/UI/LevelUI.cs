using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.ScoreSystem;
using System;
using UnityEngine.SceneManagement;
namespace CarTag.UI
{
    public class LevelUI : MonoBehaviour
    {
        
        [SerializeField] private ScoreboardUIElements scoreboardUIElements;

        private ScreenFadeUI screenFadeUI;
        private bool shouldOpenPauseMenu = true;
        private UIManager UIManager;
        private bool _isPressingReturnToMenu = false;
        private float _timer = 0;
        private float _holdTime = 1f;

        private void Awake() {
            screenFadeUI = GetComponentInChildren<ScreenFadeUI>();
        }
        private void Start() {
            UIManager = GameManager.Instance.UIManager;
        }
        // === SCOREBOARD START===
        public void DoScoreboard(PlayerScore[] playerScoresArray, bool isGameOver) {
            scoreboardUIElements.ScoreBoardGroup.gameObject.SetActive(true);
            SetScoreboardText(playerScoresArray, isGameOver);
            SetScoreboardButtons(isGameOver);
            StartCoroutine(screenFadeUI.CanvasGroupFadeRoutine(0, 1, scoreboardUIElements.ScoreBoardGroup));

        }

        public void DoPauseMenu() {
            if (!UIManager.RoundManager.IsRoundRunning) {
                return;
            }
            //--Opening Pause Menu
            if (shouldOpenPauseMenu) {
                scoreboardUIElements.ScoreBoardGroup.gameObject.SetActive(true);
                SetScoreboardButtons(true);
                Time.timeScale = 0;
                shouldOpenPauseMenu = false;
                Cursor.visible = true;
                scoreboardUIElements.MainMenuButton.Select();

            }
            //--Closing Pause Menu
            else {
                SetScoreboardButtons(false);
                scoreboardUIElements.ScoreBoardGroup.gameObject.SetActive(false);
                Cursor.visible = false;             // not working somehow
                Time.timeScale = 1;
                shouldOpenPauseMenu = true;
            }
        }

        

        public void SetScoreboardText(PlayerScore[] playerScoresArray, bool isGameOver) {
            int numberOfPlayers = playerScoresArray.Length;
            SetPlayerPositionRowsActiveState(false);                                 // disable all Player Position Rows
            SetPlayerPositionRowsActiveState(true, playerScoresArray.Length);        // enable only the required player position rows
            for (int i = 0; i < numberOfPlayers; i++) {
                //scoreboardUIElements.PlayerRowElements[i].PlayerNameText.SetText(playerScoresArray[i].PlayerScoreStats.PlayerName);
                scoreboardUIElements.PlayerRowElements[i].PlayerNameText.SetText(playerScoresArray[i].Player_Name);

                //scoreboardUIElements.PlayerRowElements[i].PlayerRoundWinsText.SetText(playerScoresArray[i].PlayerScoreStats.RoundWins.ToString());
                scoreboardUIElements.PlayerRowElements[i].PlayerRoundWinsText.SetText(playerScoresArray[i].Round_Wins.ToString());

            }
        }
        private void SetScoreboardButtons(bool hideNextRoundButton) {
            if (hideNextRoundButton) {
                scoreboardUIElements.NextRoundButton.gameObject.SetActive(false);
                scoreboardUIElements.MainMenuButton.Select();
            }
            else {
                scoreboardUIElements.NextRoundButton.gameObject.SetActive(true);
                scoreboardUIElements.NextRoundButton.Select();
            }
        }

        /// <summary>
        /// Enables or disables the Position Objects for the Scoreboard UI (The ones that display the player position , player name and player rounds won)
        /// </summary>
        /// <param name="setActive"> Set Gameobjects to true or false</param>
        /// <param name="numberOfRowsToSet"> Default is -1. In this case all Position Gameobjects will be set. Other wise the number passed in will determine the no. of objects set</param>
        private void SetPlayerPositionRowsActiveState(bool setActive, int numberOfRowsToSet = -1) {
            int rowsToSet = numberOfRowsToSet;
            if (numberOfRowsToSet == -1) {
                rowsToSet = scoreboardUIElements.PlayerRowElements.Count;
            }
            if (numberOfRowsToSet > scoreboardUIElements.PlayerRowElements.Count) {
                rowsToSet = scoreboardUIElements.PlayerRowElements.Count;
                Debug.LogError("Number of player in game is greater that the number of player which can be displayed on the scoreboard. Only player 1 to " + rowsToSet + " will be shown on scoreboard.");
            }
            for (int i = 0; i < rowsToSet; i++) {
                scoreboardUIElements.PlayerRowElements[i].PlayerPositionObject.SetActive(setActive);
            }
        }

        public void NextRoundButton() {
            scoreboardUIElements.ScoreBoardGroup.gameObject.SetActive(false);
            //Time.timeScale = 1;
            GameManager.Instance.StartNewRound();

        }
        public void ReturnToMenuButton() {
            scoreboardUIElements.ScoreBoardGroup.gameObject.SetActive(false);
            SceneManager.LoadScene(0);
        }

        public void MainMenuButtonPressed() {
            _isPressingReturnToMenu = true;
            _timer = 0;
            scoreboardUIElements.MainMenuSlider.maxValue = _holdTime;
            scoreboardUIElements.MainMenuSlider.value = 0;

        }
        private void Update() {
            if (!_isPressingReturnToMenu) { return; }
            _timer +=Time.unscaledDeltaTime;
            scoreboardUIElements.MainMenuSlider.value = _timer;
            if (_timer > _holdTime) {
                scoreboardUIElements.ScoreBoardGroup.gameObject.SetActive(false);
                SceneManager.LoadScene(0);
            }
            
        }
        public void MainMenuButtonReleased() {
            _isPressingReturnToMenu = false;
            scoreboardUIElements.MainMenuSlider.value = 0;
        }
        // === SCOREBOARD END ===
    }
}
