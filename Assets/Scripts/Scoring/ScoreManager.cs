using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.PlayerSpace;
using System.Linq;
using Sirenix.OdinInspector;

namespace CarTag.ScoreSystem {
    public class ScoreManager : MonoBehaviour {
        [SerializeField] int roundWinsToWinGame = 3;
        public List<PlayerScore> debugScores = new List<PlayerScore>();
        public PlayerManager PlayerManager { get; set; }
        //--Auto-Implemented Properties
        public List<PlayerScore> PlayerScores { get; set; }

        public void InitialSetup() {
            PlayerManager = GameManager.Instance.PlayerManager;
            PlayerScores = new List<PlayerScore>();

            for (int i = 0; i < PlayerManager.Players.Count; i++) {
                PlayerScores.Add(PlayerManager.Players[i].GetComponentInChildren<PlayerScore>());
                PlayerScores[i].roundDatas.Add(new RoundData(PlayerScores[i].ThisPlayer.IsThisPlayerCurrentRunner()));
            }
        }

        public bool UpdateScoreCheckIfGameOver(Player roundWinner) {
            //roundWinner.PlayerScore.PlayerScoreStats.RoundWins++;
            //if (roundWinner.PlayerScore.PlayerScoreStats.RoundWins >= roundWinsToWinGame) {
            roundWinner.PlayerScore.RoundWins++;
            if (roundWinner.PlayerScore.RoundWins >= roundWinsToWinGame) {
                return true;
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


        //--On Ability use need to:
        //--see what ability was used and increment the counter for that ability
        //--record the time that the ability was avalaible before being used

        //--After each Role swap need to:
        //--Increment role swap counter on each player
        //--record duration of role for each player
        //--Create a RoleData and add it to add it to list
        public void SetScoresOnRoleSwap(Player newRunner) {
            foreach (PlayerScore score in PlayerScores) {
                //--Set Role Swap Counter
                if (score.roundDatas.Count > 0) {
                    score.roundDatas[score.roundDatas.Count - 1].roleSwapsPerRound++;
                }
                else {
                    Debug.Log("Trying to increment role swap couter before list contains Round Data");
                }
                //--Record Duration of Role
                //--Create a new abilities per role and add it to add it to list
                RoleData newRoleData = new RoleData(score.ThisPlayer.IsThisPlayerCurrentRunner());
                score.roundDatas[score.roundDatas.Count - 1].roleDatas.Add(newRoleData);

            }
        }


        //--After each round need to 
        //--Add a new round data to the list


        private void Update() {
            debugScores = PlayerScores;
        }
    }
}
