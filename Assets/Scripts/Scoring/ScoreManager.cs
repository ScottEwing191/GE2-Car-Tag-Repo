using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.PlayerSpace;
using System.Linq;

namespace CarTag.ScoreSystem
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] int roundWinsToWinGame = 3;
        public PlayerManager PlayerManager { get; set; }
        //--Auto-Implemented Properties
        public List<PlayerScore> PlayerScores { get; set; }

        public void InitialSetup() {
            PlayerManager = GameManager.Instance.PlayerManager;
            PlayerScores = new List<PlayerScore>();

            for (int i = 0; i < PlayerManager.Players.Count; i++) {
                PlayerScores.Add(PlayerManager.Players[i].GetComponentInChildren<PlayerScore>());
            }
        }

        public bool UpdateScoreCheckIfGameOver(Player roundWinner) {
            roundWinner.PlayerScore.PlayerScoreStats.RoundWins++;
            if (roundWinner.PlayerScore.PlayerScoreStats.RoundWins >= roundWinsToWinGame) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns an array of Player Scores which have been sorted first place to last place.
        /// </summary>
        /// <returns></returns>
        public PlayerScore[] GetPlayerScoresInDisplayOrder() {
            var displayOrder = PlayerScores.OrderByDescending(i => i.PlayerScoreStats.RoundWins).ToArray();
            return displayOrder;
            
            
        }
    }
}
