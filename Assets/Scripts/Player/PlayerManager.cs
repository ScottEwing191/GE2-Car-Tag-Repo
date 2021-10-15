using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.PlayerSpace {
    public class PlayerManager : MonoBehaviour {
        [SerializeField] private List<Player> players = new List<Player>();

        [Tooltip("This is the time that the chaser has to wait at the start of the round before they can begin driving")]
        [SerializeField] private float initialChaserWaitTime = 3.0f;

        [Tooltip("This is the time that the chaser has to wait after a Role Swap occurs before they can begin driving again")]
        [SerializeField] private float chaserWaitTime = 4.0f;
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
        /// <summary>
        /// Takes in a game object and returns the Player script attached to the parent (or parent's parent etc) of the object
        /// </summary>
        public Player GetPlayerFromGameObject(GameObject gameObject) {
            Player playerScript = gameObject.GetComponentInParent<Player>();
            if (playerScript != null) { return playerScript; }
            else { return null; }
        }

        
        public void ControlPlayerRoleSwap(Player newRunner, Player newChaser) {
            SwapRoles(newRunner, newChaser);
            // Respawn Chasers
            
            // Change Chaser And Runner Car Stats
            
            // Turn On Collision
            StartCoroutine(newRunner.PlayerCollision.TurnCarCollisionBackOn(chaserWaitTime));
        }

        /// <summary>
        /// Swaps the Roles of the runner and the chaser which caught the runner
        /// </summary>
        /// <param name="newRunner">The car which caught the runner and will now be the new runner</param>
        /// <param name="newChaser">The car which was the old runner who got caught</param>
        private void SwapRoles(Player newRunner, Player newChaser) {
            newRunner.PlayerRoll = PlayerRoleEnum.Runner;
            newChaser.PlayerRoll = PlayerRoleEnum.Chaser;
            CurrentRunner = newRunner;
        }
    }
}
