using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Player {
    public class PlayerManager : MonoBehaviour {
        [SerializeField] private List<Player> players = new List<Player>();
        public List<Player> Players { get { return players; } }
        public Player CurrentRunner { get; set; }

        public void InitialSetup() {
            SetupPlayers();
            FindCurrentRunner();
        }

        private void SetupPlayers() {
            for (int i = 0; i < players.Count; i++) {
                players[i].InitialSetup();
                players[i].PlayerListIndex = i;             // tell each player its position in the list
            }
            foreach (var player in players) {
            }
        }

        /// <summary>
        /// Finds which one of the players in the scene is the current runner
        /// </summary>
        private void FindCurrentRunner() {
            // Gets the current Runner 
            int runners = 0;        // counts the number of runners to make sure there is only one
            foreach (var player in players) {
                if (player.PlayerRoll == PlayerRoleEnum.Runner) {
                    CurrentRunner = player;
                    runners++;
                }
            }
            if (runners > 1) {
                Debug.LogError("There are more than one Runners in the scene");
            }
            else if (runners == 0) {
                Debug.LogError("There are no Runners in the scene");
            }
        }

    }
}
