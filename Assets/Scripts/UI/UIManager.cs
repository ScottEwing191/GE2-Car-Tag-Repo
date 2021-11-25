using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.PlayerSpace;
using CarTag.Checkpoints;

namespace CarTag.UI {
    public class UIManager : MonoBehaviour {
        //--Serialized Fields
        [Tooltip("The time between counter updates as it is counting down")]
        [SerializeField] private float counterRate = 0.1f;

        [Tooltip("The min number of checkpoint markers which will be used to display the chaser progress through checkpoints")]
        [SerializeField] private int minChaserCpProgressCount = 4;
        [Tooltip("The max number of checkpoint markers which will be used to display the chaser progress through checkpoints")]
        [SerializeField] private int maxChaserCpProgressCount = 19;


        //--Private 
        private List<PlayerUIController> playerUIControllers = new List<PlayerUIController>();

        //--Auto-Implemented Properties
        public PlayerManager PlayerManager { get; private set; }
        public CheckpointManager CheckpointManager { get; private set; }
        public int cpAtRoleStart { get; set; }  // the number of checkpoints the runner has created before the chaser is allowed to start

        //--Properties
        public List<PlayerUIController> PlayerUIControllers { get { return playerUIControllers; } }

        //--Methods
        /// <summary>
        /// Using the list of player from the Player Manager find all the PlayerUIController Components and add them to the  UI Manager's list
        /// This will make sure the players are in the same order win both lists
        /// </summary>
        public void InitalSetup() {
            PlayerManager = GameManager.Instance.PlayerManager;
            CheckpointManager = GameManager.Instance.CheckpointManager;
            for (int i = 0; i < PlayerManager.Players.Count; i++) {
                playerUIControllers.Add(PlayerManager.Players[i].GetComponentInChildren<PlayerUIController>());
                playerUIControllers[i].InitialSetup();
            }
           /* foreach (Player player in PlayerManager.Players) {
                playerUIControllers.Add(player.GetComponentInChildren<PlayerUIController>());
            }*/
            ResetUI();
        }

        //=== COUNTDOWN TIMER START ===
        /// <summary>
        /// Starts the countdown timer on the runner
        /// </summary>
        public void StartRunnerCountdown(float startTime) {
            PlayerUIController runnerUI = playerUIControllers[PlayerManager.CurrentRunner.PlayerListIndex];
            StartCoroutine(runnerUI.DoCountdownTimer(startTime, counterRate, 1));
        }

        /// <summary>
        /// Starts the countdown timer on the Chasers. Skips the runner
        /// </summary>
        public void StartChaserCountdown(float startTime) {
            for (int i = 0; i < playerUIControllers.Count; i++) {
                if (i != PlayerManager.CurrentRunner.PlayerListIndex) {
                    StartCoroutine(playerUIControllers[i].DoCountdownTimer(startTime, counterRate, 2));
                }
            }
        }
        //=== COUNTDOWN TIMER END ===

        public PlayerUIController GetRunnerUIController() {
            return playerUIControllers[PlayerManager.CurrentRunner.PlayerListIndex];
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentDst"></param>
        /// <param name="dstBetweenCheckpoint"></param>
        /// <param name="spawned"></param>
        /// <param name="runnerCheckpointsAhead">The no. of cp's the runner is ahead of the leading chaser</param>
        public void ManageCheckpointSpawnUI(float currentDst, float dstBetweenCheckpoint, bool spawned = true, int runnerCheckpointsAhead = -1) {
            //--Set RunnerSlider regardless of whether a new checkpoint was spawned or not
            GetRunnerUIController().SetPlacedCheckpointTracker(currentDst, dstBetweenCheckpoint);
            if (spawned) {
                GetRunnerUIController().SetCheckpointsAheadText(runnerCheckpointsAhead);    // set runner checkpoints ahead counter
                UpdateChaserCheckpointTrackers();
            }

        }

        private void UpdateChaserCheckpointTrackers() {
            for (int i = 0; i < playerUIControllers.Count; i++) {
                if (playerUIControllers[i] != GetRunnerUIController()) {
                    int checkpointsLeft = CheckpointManager.CheckpointQueues[i].Count;
                    playerUIControllers[i].ChaserCheckpointTracker.UpdateTracker(checkpointsLeft);
                }
            }
                
        }

        public void ManageCheckpointReachedUI(int runnerCheckpointsAhead) {
            //--Runner
            GetRunnerUIController().SetCheckpointsAheadText(runnerCheckpointsAhead);
            //--Chaser
            UpdateChaserCheckpointTrackers();
        }

        /// <summary>
        /// This is a called after the runner has had there head start and should be a few checkpoints ahead.
        /// Sets up the Chaser's UI whick tracks how checkpoints behind they are
        /// </summary>
        public void SetupChaserCheckpointTrackers() {
            //--Get the count of any of the chaser checkpointQueues since they will all be the same at the stage where they have just been enabled...
            //... after a Role swap or round start
            int checkpointsMade = -1;
            for (int i = 0; i < CheckpointManager.CheckpointQueues.Count; i++) {
                if (i!= PlayerManager.CurrentRunner.PlayerListIndex) {
                    checkpointsMade = CheckpointManager.CheckpointQueues[i].Count;
                    break;
                }
            }
            if (checkpointsMade == -1) { Debug.LogError("Error: Chaser Checkpoint setup with -1 checkpoints created"); }
            //--Setup the tracker on each chaser
            foreach (var p in playerUIControllers) {
                if (p != GetRunnerUIController()) {
                    p.ChaserCheckpointTracker.SetupCpTracker(checkpointsMade);
                }
            }
        }

        
        //=== ROLE/ROUND CHANGE ===
        public void RoleSwapReset(Player newRunner, Player newChaser) {
            ResetUI();

        }
        /*public void RoundSwapScreenFade(Player newRunner, Player newChaser) {
            // Start
        }*/
        internal void RoundStartReset() {
            ResetUI();
        }

        private void ResetUI() {
            foreach (var p in playerUIControllers) {
                p.SetCheckpointsAheadText(0);                       // reset runner's checkpoints ahead tracker
                p.SetPlacedCheckpointTracker(0, 0);                 // reset runner's checkpoint placed slider
                
                p.ChaserCheckpointTracker.ResetCpTracker();         // reset chaser's checkpoint tracker
                // Ability UI Is Reset from the PlayerAbilityController
                p.SwitchToChaserUI();                               // switch all cars to chaser UI
            }
            GetRunnerUIController().SwitchToRunnerUI();             // switch only the runner to Runner UI
        }

        //=== UPDATE RUNNER DISTANCE TRAVELLED ===
        public void UpdateRunnerDistanceTracker(float distanceTravelled, float targetDistance) {
            GetRunnerUIController().SetDistanceTrackerUI(distanceTravelled, targetDistance);
            foreach (var p in playerUIControllers) {
                if (p != GetRunnerUIController()) {
                    p.SetDistanceTrackerChaserUI(distanceTravelled, targetDistance);
                }
            }
        }

    }
}
