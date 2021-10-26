using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Car;

namespace CarTag.PlayerSpace {
    public class PlayerManager : MonoBehaviour {
        [SerializeField] private List<Player> players = new List<Player>();

        [Tooltip("This is the time that the chaser has to wait at the start of the round before they can begin driving")]
        [SerializeField] private float initialChaserWaitTime = 3.0f;

        [Tooltip("This is the time that the chaser has to wait after a Role Swap occurs before they can begin driving again")]
        [SerializeField] private float chaserWaitTime = 4.0f;

        private CarStatsController carStatsController;

        //--Auto Implemented Properties
        public Player CurrentRunner { get; set; }

        //--Properties
        public List<Player> Players { get { return players; } }

        //=== SET UP START ===

        public void InitialSetup() {
            carStatsController = GetComponent<CarStatsController>();
            SetupPlayers();
            FindCurrentRunner();
            AssignCarStats();
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
        /// Assign the Runner or Chaser stats to the appropriate car controllers
        /// </summary>
        private void AssignCarStats() {
            foreach (Player p in players) {
                if (p == CurrentRunner) {
                    carStatsController.AssignStats(p.CarController, carStatsController.RunnerStats);
                }
                else {
                    carStatsController.AssignStats(p.CarController, carStatsController.ChaserStats);

                }
            }
        }
        //=== SET UP END ===

        /// <summary>
        /// Takes in a game object and returns the Player script attached to the parent (or parent's parent etc) of the object
        /// </summary>
        public Player GetPlayerFromGameObject(GameObject gameObject) {
            Player playerScript = gameObject.GetComponentInParent<Player>();
            if (playerScript != null) { return playerScript; }
            else { return null; }
        }

        //=== ROLE SWAP START ===
        public void ControlPlayerRoleSwap(Player newRunner, Player newChaser) {
            SwapRoles(newRunner, newChaser);
            RespawnChasers(newRunner, newChaser);
            SwapCarStats(newRunner, newChaser);

            // Change Chaser And Runner Car Stats

            // Turn On Collision
            StartCoroutine(newRunner.PlayerCollision.TurnOnCarCollision(chaserWaitTime));
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

        /// <summary>
        /// Respawn all cars which are not the new Runner or the new Chaser at the new Chaser's Position (if there are only two players this method will ultimetly 
        /// do nothing)
        /// </summary>
        private void RespawnChasers(Player newRunner, Player newChaser) {
            Vector3 respawnPos = newChaser.PlayerRespawn.transform.position;
            Quaternion respawnRot = newChaser.PlayerRespawn.transform.rotation;
            for (int i = 0; i < players.Count; i++) {
                if (players[i] == newRunner || players[i] == newChaser) {                   // dont need to respawn runner or chaser
                    players[i].PlayerRespawn.SetRespawnLocation(respawnPos, respawnRot);    // set respawn Location of Runner and chaser without
                    continue;                                                               // ...actually respawning them
                }
                else {
                    players[i].PlayerRespawn.RespawnAfterRoleSwap(respawnPos, respawnRot);  // respawn chasers at new chaser pos & rot
                }
            }
        }
        private void SwapCarStats(Player newRunner, Player newChaser) {
            carStatsController.AssignStats(newRunner.CarController, carStatsController.RunnerStats);
            carStatsController.AssignStats(newChaser.CarController, carStatsController.ChaserStats);
        }

        //=== ROLE SWAP END ===

        //=== ENABLE/DISABLE CARS START ===
        public void DisableCars() {
            foreach (Player p in players) {
                carStatsController.DisableCar(p.CarController);
            }
        }
        public void EnableRunner() {
            carStatsController.EnableCar(CurrentRunner.CarController, carStatsController.RunnerStats);
        }
        public void EnableChasers() {
            carStatsController.EnableCar(CurrentRunner.CarController, carStatsController.RunnerStats);
            foreach (Player p in players) {
                if (p != CurrentRunner) {
                    carStatsController.EnableCar(p.CarController, carStatsController.ChaserStats);
                }
            }
        }
        //=== ENABLE/DISABLE CARS END ===

    }
}
