using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.PlayerSpace;
using System.Linq;
using Sirenix.OdinInspector;
using System;

namespace CarTag.ScoreSystem {
    [System.Serializable]
    public class ScoreManager : MonoBehaviour {
        [SerializeField] int roundWinsToWinGame = 3;
        public PlayerManager PlayerManager { get; set; }
        public PlayerScore[] playerScoresArray;
        //--Auto-Implemented Properties
        //public List<PlayerScore> PlayerScores { get; set; }
        public List<PlayerScore> PlayerScores = new List<PlayerScore>();


        public void InitialSetup() {
            PlayerManager = GameManager.Instance.PlayerManager;
            //PlayerScores = new List<PlayerScore>();
            string testType = GetTestType();
            for (int i = 0; i < PlayerManager.Players.Count; i++) {
                PlayerScores.Add(PlayerManager.Players[i].GetComponentInChildren<PlayerScore>());
                //-- TELEMETRY CODE ---
                PlayerScores[i].Round_Data.Add(new RoundData(PlayerScores[i].ThisPlayer.IsThisPlayerCurrentRunner(), PlayerScores[i].Round_Data.Count + 1));
                PlayerScores[i].Test_Type = testType;
            }
            //=== Setting Head Start For A/B Testing. Not A Good Place To Do This
            //TURN BACK ON FOR TESTING
            SetABTestingValues(testType);
        }

        private void OnEnable() {
            GameEvents.onRoleSwapNullary += SetScoresOnRoleSwap;
            GameEvents.onRoundResetNullary += SetupScoresForNewRound;
        }

        private void OnDisable() {
            GameEvents.onRoleSwapNullary -= SetScoresOnRoleSwap;
            GameEvents.onRoundResetNullary -= SetupScoresForNewRound;

        }

        private void SetABTestingValues(string testType) {
            int chaserStartWait = 5;            // A test Values
            int chaserSwapWait = 6;
            if (testType == "B Test") {
                chaserStartWait = 2;
                chaserSwapWait = 3;
            }

            GameManager.Instance.RoundManager.ChaserStartWaitTime = chaserStartWait;
            PlayerManager.ChaserRoleSwapStartWaitTime = chaserSwapWait;
            PlayerManager.ChaserRoleSwapWaitTime_ = chaserSwapWait;
        }

        private string GetTestType() {
            MainMenu.AB_TestButtons test = FindObjectOfType<MainMenu.AB_TestButtons>();
            string type = "B Test";
            if (test != null) {
                type = test.SelectedTest;
                Destroy(test.gameObject);
            }
            return type;
        }



        public bool UpdateScoreCheckIfGameOver(Player roundWinner) {
            //roundWinner.PlayerScore.PlayerScoreStats.RoundWins++;
            //if (roundWinner.PlayerScore.PlayerScoreStats.RoundWins >= roundWinsToWinGame) {

            roundWinner.PlayerScore.Round_Wins++;
            if (roundWinner.PlayerScore.Round_Wins >= roundWinsToWinGame) {
                SetAllPlayersRoleDuration();
                //Save Telemetry Data to file
                SaveManager.SaveTelemetryData(PlayerScores);
                //SaveManager.SaveToNewFile(PlayerScores);
                //SaveManager.SaveTelemetryData(this);
                return true;
            }
            return false;
        }

        public void SetupScoresForNewRound() {
            //--Add new round data if there is going to be another round
            /*foreach (PlayerScore score in PlayerScores) {
                score.SetRoleDuration();
                RoundData roundData = new RoundData(score.ThisPlayer.IsThisPlayerCurrentRunner());
                score.Round_Data.Add(roundData);
            }*/
            for (int i = 0; i < PlayerScores.Count; i++) {
                PlayerScores[i].SetRoleDuration();
                RoundData roundData = new RoundData(PlayerScores[i].ThisPlayer.IsThisPlayerCurrentRunner(), PlayerScores[i].Round_Data.Count + 1);
                PlayerScores[i].Round_Data.Add(roundData);
            }
        }

        /// <summary>
        /// Returns an array of Player Scores which have been sorted first place to last place.
        /// </summary>
        public PlayerScore[] GetPlayerScoresInDisplayOrder() {
            //var displayOrder = PlayerScores.OrderByDescending(i => i.PlayerScoreStats.RoundWins).ToArray();
            var displayOrder = PlayerScores.OrderByDescending(i => i.Round_Wins).ToArray();

            return displayOrder;


        }




        //--Called from GameManager on role swap
        public void SetScoresOnRoleSwap() {
            foreach (PlayerScore score in PlayerScores) {
                //--Set Role Swap Counter
                if (score.Round_Data.Count > 0) {
                    score.Round_Data[score.Round_Data.Count - 1].Role_Swaps_Per_Round++;
                }
                else {
                    Debug.Log("Trying to increment role swap counter before list contains Round Data");
                }
                //--Record Duration of Role
                score.SetRoleDuration();
                //--Create a new abilities per role and add it to add it to list
                RoleData newRoleData = new RoleData(score.ThisPlayer.IsThisPlayerCurrentRunner());
                score.Round_Data[score.Round_Data.Count - 1].Role_Data.Add(newRoleData);

            }
        }

        void SetAllPlayersRoleDuration() {
            foreach (PlayerScore score in PlayerScores) {
                score.SetRoleDuration();
            }
        }
        internal void GameForfeited() {
            foreach (PlayerScore score in PlayerScores) {
                score.GetCurrentRoundData().forfeit = true;
            }
        }
    }
}
