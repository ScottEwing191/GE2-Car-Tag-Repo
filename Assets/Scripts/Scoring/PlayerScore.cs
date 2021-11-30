using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.ScoreSystem
{
    //--There is one PlayerScore script attached to each player
    //--In future this could be used to track all sorts of information about the player eg., Abilities used, times hit distance/time spent as runner, time drifting
    public class PlayerScore : MonoBehaviour
    {
        Player thisPlayer;
        private PlayerScoreStats playerScoreStats = new PlayerScoreStats();
        public PlayerScoreStats PlayerScoreStats {
            get { return playerScoreStats; }
        }

        //--If I want to replace this if I let the player enter their own names
        private void Start() {
            thisPlayer = GetComponentInParent<Player>();
            PlayerScoreStats.PlayerName = thisPlayer.gameObject.name;
        }
    }
}
