using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.PlayerSpace;
using CarTag.Road;
using CarTag.Checkpoints;
using CarTag.UI;
using CarTag.Abilities;
using CarTag.ScoreSystem;
using CarTag.Rounds;


namespace CarTag {
    public class GameManager : MonoSingleton<GameManager> {
        public PlayerManager PlayerManager { get; set; }
        public RoadManager RoadManager { get; set; }
        public CheckpointManager CheckpointManager { get; set; }
        public RoundManager RoundManager { get; set; }
        public UIManager UIManager { get; set; }
        public AbilityManager AbilityManager { get; set; }
        public ScoreManager ScoreManager { get; set; }

        protected override void Awake() {
            base.Awake();
            PlayerManager = FindObjectOfType<PlayerManager>();
            RoadManager = FindObjectOfType<RoadManager>();
            CheckpointManager = FindObjectOfType<CheckpointManager>();
            RoundManager = FindObjectOfType<RoundManager>();
            UIManager = FindObjectOfType<UIManager>();
            AbilityManager = FindObjectOfType<AbilityManager>();
            ScoreManager = FindObjectOfType<ScoreManager>();
        }

        private void Start() {
            //PreInitialSetup();          // Make sure all players are in the game and controllers are all working (NOT DONE YET)
            InitialSetup();
        }

        //--Will make sure that the controllers for all players in the game are working
        /*private void PreInitialSetup() {
            PlayerManager.PreInitialSetup();
        }*/
        /// <summary>
        /// This starts the inital for the game including setting up the Player And road managers which in turn initiate setup on other scripts
        /// </summary>
        private void InitialSetup() {
            PlayerManager.InitialSetup();                                //Setup player Runners
            RoadManager.InitialSetup(PlayerManager.CurrentRunner.RoadSpawnData); 
            CheckpointManager.InitialSetup(PlayerManager.Players.Count);
            ScoreManager.InitialSetup();                //must come before UI Manager
            UIManager.InitalSetup();                    // this need to come before PlayerManager
            AbilityManager.InitialSetup();              // was Before UI Manager. Dont remember if that was required for something. It now must be after the UI Manager due to...
                                                        //... PlayerAbilityController InitalSetup() Calling the UI System.
            RoundManager.InitalSetup();
            StartCoroutine(RoundManager.RoundStart());
        }


        /// <summary>
        /// This is called when the chaser catches the runner. It Initiate the Role Swap code in various classes in the game 
        /// </summary>
        /// <param name="newRunner">The Player Script on the new runner (old chaser)</param>
        /// <param name="newChaser">The Player Script on the new chaser (old runner)</param>
        internal void ManageRoleSwap(Player newRunner, Player newChaser) {
            GameEvents.RoleSwap(newRunner, newChaser);
            //PlayerManager.ControlPlayerRoleSwap(newRunner, newChaser);          // Needs To be first 
            //RoadManager.ResetRoad();                                            // Position Shouldnt matter??
            //CheckpointManager.ResetCheckpoints();                               // Position Shouldnt Matter
            //UIManager.RoleSwapReset(newRunner, newChaser);                      // Position Shouldn't Matter
            //ScoreManager.SetScoresOnRoleSwap();                                 // Position Shouldn't Matter
            //AbilityManager.ResetAbilities(newRunner);                           // Needs to come after Player. Accesses player.IsThisPlayerCurrentRunner()
        }

        public void ManageRoundOver() {
            Cursor.visible = true;
            PlayerManager.DisableCars();                                // display cars while scoreboard is up
            bool gameOver = ScoreManager.UpdateScoreCheckIfGameOver(PlayerManager.CurrentRunner);        // update scores after round
            // If gameOver is true then the UI button which the player presses to advance to the next round will be disabled leaving a button to return to Menu
            UIManager.ShowScores(gameOver);                                     // display scoreboard
        }

        public void StartNewRound() {
            StartCoroutine(StartNewRoundRoutine());
        }

        private IEnumerator StartNewRoundRoutine() {
            yield return StartCoroutine(UIManager.ScreenFadeOnAllPlayers(0, 1, ScreenFadeUI.DEFAULT_FADE_TIME));        // Return when screen has faded to black
            GameEvents.RoundReset(PlayerManager.GetNextRoundRunner());
            //--Reset Managers For New Round
            //PlayerManager.ResetPlayersAfterRound();
            
            RoadManager.ResetRoad();
            CheckpointManager.ResetCheckpoints();
            UIManager.RoundStartReset();                                    // this need to come after PlayerManager
            AbilityManager.ResetAbilities(PlayerManager.CurrentRunner);     // this need to come after PlayerManager
            ScoreManager.SetupScoresForNewRound();
            
            yield return new WaitForSeconds(0.5f);                                                                      // Pause on black screen 
            yield return StartCoroutine(UIManager.ScreenFadeOnAllPlayers(1, 0, ScreenFadeUI.DEFAULT_FADE_TIME));        // Return when screen has faded to translucent
            StartCoroutine(RoundManager.RoundStart());
            yield return null;
        }

    }
}
