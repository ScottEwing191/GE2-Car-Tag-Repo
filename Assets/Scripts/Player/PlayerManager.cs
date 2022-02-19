using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Car;
using CarTag.UI;

namespace CarTag.PlayerSpace {
    public class PlayerManager : MonoBehaviour {
        [SerializeField] private List<GameObject> playerObjects = new List<GameObject>();
        private List<Player> players = new List<Player>();
        [SerializeField] private float chaserRoleSwapStartWaitTime = 5.0f;
        [Tooltip("This is the time that the chaser has to wait after a Role Swap occurs before they can begin driving again")]
        private float chaserRoleSwapWaitTime = 5.0f;
        [Tooltip("The time the chaser has to wait after a role swap get higher each time. This is the amount the time increases by each time")]
        [SerializeField] private float increaseChaserWaitTimeBy = 1;
        [SerializeField] private int defaultPlayersInGame = 2;

        private CarStatsController carStatsController;
        private Player runnerAtRoundStart;

        //--Auto Implemented Properties
        public Player CurrentRunner { get; set; }
        public UIManager UIManager { get; set; }


        //--Properties
        public List<Player> Players { get { return players; } }
        //=== JUST REQUIRED FOR TELEMETRY ===
        public float ChaserRoleSwapStartWaitTime {
            get { return chaserRoleSwapStartWaitTime; }
            set { chaserRoleSwapStartWaitTime = value; }
        }

        //=== SET UP START ===

        /*public void PreInitialSetup() {
            SetupPlayersList();
            // Get each player to Input Key to make sure controller is working
        }*/

        public void InitialSetup() {
            UIManager = GameManager.Instance.UIManager;
            carStatsController = GetComponent<CarStatsController>();
            SetupPlayersList();
            SetupPlayers();
            FindCurrentRunner();
            runnerAtRoundStart = CurrentRunner;
            //AssignCarStats();
            ChangeAllCars();
            chaserRoleSwapWaitTime = chaserRoleSwapStartWaitTime;
        }

        //--Use the number of players selected in the Main menu to disable the player Objects which are not required and remove them from te players list.
        private void SetupPlayersList() {
            int playersInGame = -1;
            MainMenu.PlayersPlaying playersPlaying = FindObjectOfType<MainMenu.PlayersPlaying>();
            if (playersPlaying != null) {
                playersInGame = playersPlaying.NumberOfPlayers;
                Destroy(playersPlaying.gameObject);
            }
            else {
                //Debug.LogError("Players Playing script from the Main Menu was Not found");
                playersInGame = defaultPlayersInGame;
            }
            for (int i = 0; i < playerObjects.Count; i++) {
                playerObjects[i].SetActive(false);
            }
            //--Add the desired number of players list enable those players
            for (int i = 0; i < playersInGame; i++) {
                players.Add(playerObjects[i].GetComponent<Player>());
                players[i].gameObject.SetActive(true);
            }
        }
        private void SetupPlayers() {
            for (int i = 0; i < players.Count; i++) {
                players[i].InitialSetup();
                players[i].PlayerListIndex = i;             // tell each player its position in the list
            }
        }

        /// Finds which one of the players in the scene is the current runner
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
            InvokeRoleSwapEvents();
            SwapRoles(newRunner, newChaser);
            RespawnChasers(newRunner, newChaser);
            SwapCars(newRunner, newChaser);
            DisableChasers();
            StartCoroutine(newRunner.PlayerCollision.TurnOnCarCollision(chaserRoleSwapWaitTime));
            UIManager.StartChaserCountdown(chaserRoleSwapWaitTime);
            StartCoroutine(StartChasersAfterRoleSwapWait());
            chaserRoleSwapWaitTime += increaseChaserWaitTimeBy;         // increase the time the chaser will have to wait for the next role swap
        }

        private void InvokeRoleSwapEvents() {
            foreach (var p in players) {
                p.InvokeRoleSwapEvent();
            }
        }

        private void SwapCars(Player newRunner, Player newChaser) {
            newRunner.ChangePlayerCars.ChangeCar(true);
            newChaser.ChangePlayerCars.ChangeCar(false);

        }

        private void ChangeAllCars() {
            foreach (Player player in players) {
                player.ChangePlayerCars.ChangeCar(player.IsThisPlayerCurrentRunner(), false);
            }
        }

        /// <summary>
        /// Swaps the Roles of the runner and the chaser which caught the runner
        /// </summary>
        /// <param name="newRunner">The car which caught the runner and will now be the new runner</param>
        /// <param name="newChaser">The car which was the old runner who got caught</param>
        private void SwapRoles(Player newRunner, Player newChaser) {
            newChaser.PlayerRoll = PlayerRoleEnum.Chaser;
            newRunner.PlayerRoll = PlayerRoleEnum.Runner;
            CurrentRunner = newRunner;
        }

        /// <summary>
        /// Respawn all cars which are not the new Runner or the new Chaser at the new Chaser's Position (if there are only two players this method will ultimetly 
        /// do nothing)
        /// </summary>
        private void RespawnChasers(Player newRunner, Player newChaser) {
            Vector3 respawnPos = newChaser.RCC_CarController.transform.position;
            Quaternion respawnRot = newChaser.RCC_CarController.transform.rotation;

            for (int i = 0; i < players.Count; i++) {
                if (players[i] == newRunner ) {                                             // dont need to respawn runner
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
                p.DisablePlayer();
            }
        }
        private void DisableChasers() {
            foreach (Player p in players) {
                if (p != CurrentRunner) {
                    p.DisablePlayer();
                }
            }
        }

        public void EnableRunner() {
            CurrentRunner.EnablePlayer();
        }
        public void EnableChasers() {
            foreach (Player p in players) {
                if (p != CurrentRunner) {
                    p.EnablePlayer();
                }
            }
        }
        //=== ENABLE/DISABLE CARS END ===

        public void ResetPlayersAfterRound() {
            foreach (var p in players) {
                p.PlayerRespawn.RespawnAfterRound();
                p.InvokeRoundEndEvent();                    // tell player to invoke the round end event
            }
            ResetRolesAfterRound();
            ChangeAllCars();
            chaserRoleSwapWaitTime = chaserRoleSwapStartWaitTime;
        }
        /// <summary>
        /// Sets the player who will be the runner at the start of the next rond.
        /// The players take turns at being the runner in the order that they appear in the player List.
        /// The winner of the previous round has no effect on the start runner of the next round 
        /// </summary>
        private void ResetRolesAfterRound() {
            if (runnerAtRoundStart.PlayerListIndex == players.Count - 1) {
                runnerAtRoundStart = players[0];
            }
            else {
                runnerAtRoundStart = players[(runnerAtRoundStart.PlayerListIndex + 1)];
            }
            SwapRoles(runnerAtRoundStart, CurrentRunner);
        }

        /// <summary>
        /// Allows the chasers to begin driving again after the Role Swap
        /// Tells UI Manager to Set up the chaser Checkpoint Tracker UI
        /// </summary>
        private IEnumerator StartChasersAfterRoleSwapWait() {
            yield return new WaitForSeconds(chaserRoleSwapWaitTime);
            EnableChasers();
            UIManager.SetupChaserCheckpointTrackers();
        }
    }
}
