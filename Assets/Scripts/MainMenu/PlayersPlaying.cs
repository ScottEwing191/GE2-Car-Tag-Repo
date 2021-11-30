using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.MainMenu
{
    public class PlayersPlaying : MonoBehaviour
    {
        public int NumberOfPlayers { get; set; }
        private void Awake() {
            NumberOfPlayers = 2;
            DontDestroyOnLoad(this.gameObject);
            print("Player: " + NumberOfPlayers);
        }
    }
}
