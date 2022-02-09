using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.PlayerSpace;
using System.Linq;
using Sirenix.OdinInspector;

namespace CarTag.ScoreSystem {
    public class ScoreManager : MonoBehaviour {
        [SerializeField] int roundWinsToWinGame = 3;
        public PlayerManager PlayerManager { get; set; }

        //--Auto-Implemented Properties
        public List<PlayerScore> PlayerScores { get; set; }

        public void InitialSetup() {
            PlayerManager = GameManager.Instance.PlayerManager;
            PlayerScores = new List<PlayerScore>();

            for (int i = 0; i < PlayerManager.Players.Count; i++) {
                PlayerScores.Add(PlayerManager.Players[i].GetComponentInChildren<PlayerScore>());
                PlayerScores[i].roundData.Add(new RoundData(PlayerScores[i].ThisPlayer.IsThisPlayerCurrentRunner()));
            }
        }

        private void Update() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.P)) {
                SaveManager.Save(PlayerScores[0]);
                SaveManager.Save(PlayerScores[1]);

                //foreach (var s in PlayerScores) {
                //    SaveManager.Save(s, s.PlayerName);
                //}

            }
        }

        public bool UpdateScoreCheckIfGameOver(Player roundWinner) {
            //roundWinner.PlayerScore.PlayerScoreStats.RoundWins++;
            //if (roundWinner.PlayerScore.PlayerScoreStats.RoundWins >= roundWinsToWinGame) {

            roundWinner.PlayerScore.RoundWins++;
            if (roundWinner.PlayerScore.RoundWins >= roundWinsToWinGame) {
                return true;
            }
            //--Add new round data if there is going to be another round
            foreach (PlayerScore score in PlayerScores) {
                RoundData roundData = new RoundData(score.ThisPlayer.IsThisPlayerCurrentRunner());
                score.roundData.Add(roundData);
            }
            return false;
        }

        /// <summary>
        /// Returns an array of Player Scores which have been sorted first place to last place.
        /// </summary>
        public PlayerScore[] GetPlayerScoresInDisplayOrder() {
            //var displayOrder = PlayerScores.OrderByDescending(i => i.PlayerScoreStats.RoundWins).ToArray();
            var displayOrder = PlayerScores.OrderByDescending(i => i.RoundWins).ToArray();

            return displayOrder;


        }




        //--Called from GameManager on role swap
        public void SetScoresOnRoleSwap(Player newRunner) {
            foreach (PlayerScore score in PlayerScores) {
                //--Set Role Swap Counter
                if (score.roundData.Count > 0) {
                    score.roundData[score.roundData.Count - 1].roleSwapsPerRound++;
                }
                else {
                    Debug.Log("Trying to increment role swap couter before list contains Round Data");
                }
                //--Record Duration of Role
                score.SetRoleDuration();
                //--Create a new abilities per role and add it to add it to list
                RoleData newRoleData = new RoleData(score.ThisPlayer.IsThisPlayerCurrentRunner());
                score.roundData[score.roundData.Count - 1].roleData.Add(newRoleData);

            }
        }
    }
}
