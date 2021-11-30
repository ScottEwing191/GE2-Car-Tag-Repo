using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CarTag.MainMenu
{
    public class SelectPlayersButtons : MonoBehaviour
    {
        [SerializeField] PlayersPlaying playersPlaying;
        private bool selected = false;                      // stops the player from double clicking buttons
        
        public void SelectTwoPlayers() {
            if (!selected) {
                selected = true;
                playersPlaying.NumberOfPlayers = 2;
                SceneManager.LoadScene("TestLevel");
            }
        }
        public void SelectThreePlayers() {
            if (!selected) {
                selected = true;
                playersPlaying.NumberOfPlayers = 3;
                SceneManager.LoadScene("TestLevel");
            }
        }
        public void SelectFourPlayers() {
            if (!selected) {
                selected = true;
                playersPlaying.NumberOfPlayers = 4;
                SceneManager.LoadScene("TestLevel");
            }
        }
    }
}
