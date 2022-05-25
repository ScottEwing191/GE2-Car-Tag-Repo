using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.PlayerSpace;
using CarTag.Checkpoints;
using CarTag.ScoreSystem;
using CarTag.Rounds;


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
        //private LevelUI LevelUI;

        //--Auto-Implemented Properties
        public PlayerManager PlayerManager { get; private set; }
        public CheckpointManager CheckpointManager { get; private set; }
        public ScoreManager ScoreManager { get; set; }
        public RoundManager RoundManager { get; private set; }
        public LevelUI LevelUI { get; set; }

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
            ScoreManager = GameManager.Instance.ScoreManager;
            RoundManager = GameManager.Instance.RoundManager;
            LevelUI = FindObjectOfType<LevelUI>();
            for (int i = 0; i < PlayerManager.Players.Count; i++) {
                playerUIControllers.Add(PlayerManager.Players[i].GetComponentInChildren<PlayerUIController>());
                playerUIControllers[i].InitialSetup();
            }
            ResetUI(PlayerManager.CurrentRunner);
            LevelUI.SetScoreboardText(ScoreManager.GetPlayerScoresInDisplayOrder(), false);
        }

        private void OnEnable() {
            GameEvents.onRoleSwap += RoleSwapReset;
            //GameEvents.onRoundReset += RoundEndReset;

        }

        private void OnDisable() {
            GameEvents.onRoleSwap -= RoleSwapReset;
            //GameEvents.onRoundReset -= RoundEndReset;

        }

        // === SCOREBOARD ===
        public void ShowScores(bool isGameOver) {
            DisablePlayersUI();
            LevelUI.DoScoreboard(ScoreManager.GetPlayerScoresInDisplayOrder(), isGameOver);
        }

        // === SCREEN FADE ===
        /// <summary>
        /// Fades the UI screen for each player from one alpha value to another. 
        /// </summary>
        /// <param name="fromAlpha">The start alpha value of the image</param>
        /// <param name="toAlpha">The end alpha value of the image</param>
        /// <param name="waitTime">Time to wait after each of the player have been told to start fading befor returning from method</param>
        /// <returns></returns>
        public IEnumerator ScreenFadeOnAllPlayers(int fromAlpha, int toAlpha, float waitTime) {
            foreach (var p in playerUIControllers) {
                StartCoroutine(p.ScreenFadeUI.ScreenFadeRoutine(fromAlpha, toAlpha));          
            }
            yield return new WaitForSeconds(waitTime);

        }

        //=== COUNTDOWN TIMER START ===
        /// <summary>
        /// Starts the countdown timer on the runner
        /// </summary>
        public void StartRunnerCountdown(float startTime) {
            PlayerUIController runnerUI = playerUIControllers[PlayerManager.CurrentRunner.PlayerListIndex];
            //StartCoroutine(runnerUI.DoCountdownTimerRoutine(startTime, counterRate, 1));
            runnerUI.DoCountdownTimer(startTime, counterRate, 1);

        }

        /// <summary>
        /// Starts the countdown timer on the Chasers. Skips the runner
        /// </summary>
        public void StartChaserCountdown(float startTime) {
            for (int i = 0; i < playerUIControllers.Count; i++) {
                if (i != PlayerManager.CurrentRunner.PlayerListIndex) {
                    //StartCoroutine(playerUIControllers[i].DoCountdownTimerRoutine(startTime, counterRate, 2));
                    playerUIControllers[i].DoCountdownTimer(startTime, counterRate, 2);
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
            ResetUI(newRunner);

        }
        internal void RoundEndReset(Player newRunner) {
            ResetUI(newRunner);
        }

        private void ResetUI(Player newRunner) {
            newRunner.PlayerUIController.ResetUI(true);
            foreach (var p in playerUIControllers) {
                if (p.thisPlayer == newRunner) 
                    continue;                  
                p.ResetUI(false);
            }
        }
        //=== ROLE/ROUND CHANGE END ===

        //--Disables the runner/ chaser/ player UI for all players in the game
        private void DisablePlayersUI() {
            foreach (var p in playerUIControllers) {
                p.DisableUI();
            }
        }

        //=== UPDATE RUNNER DISTANCE TRAVELLED ===
        public void UpdateDistanceTracker(float distanceTravelled, float targetDistance) {
            foreach (var p in playerUIControllers) {
                p.SetDistanceTrackerUI(distanceTravelled,targetDistance);
            }
        }

    }
}
