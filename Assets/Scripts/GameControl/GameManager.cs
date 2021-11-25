using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.PlayerSpace;
using CarTag.Road;
using CarTag.Checkpoints;
using CarTag.UI;
using CarTag.Abilities;

namespace CarTag
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public PlayerManager PlayerManager { get; set; }
        public RoadManager RoadManager { get; set; }
        public CheckpointManager CheckpointManager { get; set; }
        public RoundManager RoundManager { get; set; }
        public UIManager UIManager { get; set; }
        public AbilityManager AbilityManager { get; set; }

        private void Start() {
            InitialSetup();
        }

        /// <summary>
        /// This starts the inital for the game including setting up the Player And road managers which in turn initiate setup on other scripts
        /// </summary>
        private void InitialSetup() {
            PlayerManager = FindObjectOfType<PlayerManager>();
            RoadManager = FindObjectOfType<RoadManager>();
            CheckpointManager = FindObjectOfType<CheckpointManager>();
            RoundManager = FindObjectOfType<RoundManager>();
            UIManager = FindObjectOfType<UIManager>();
            AbilityManager = FindObjectOfType<AbilityManager>();

            PlayerManager.InitialSetup();                                //Setup player Runners
            RoadManager.InitialSetup(PlayerManager.CurrentRunner.RoadSpawnData);
            CheckpointManager.InitialSetup(PlayerManager.Players.Count);
            AbilityManager.InitialSetup();
            UIManager.InitalSetup();
            RoundManager.InitalSetup();
            StartCoroutine(RoundManager.RoundStart());
        }


        /// <summary>
        /// This is called when the chaser catches the runner. It Initiate the Role Swap code in various classes in the game 
        /// </summary>
        /// <param name="newRunner">The Player Script on the new runner (old chaser)</param>
        /// <param name="newChaser">The Player Script on the new chaser (old runner)</param>
        internal void ManageRoleSwap(Player newRunner, Player newChaser) {
            //StartCoroutine(ContinueManageRoleSwap(newRunner, newChaser));
            PlayerManager.ControlPlayerRoleSwap(newRunner, newChaser);          // Start Player Manager Role Swap Code
            RoadManager.ResetRoad(newRunner.RoadSpawnData);               // Start Road Manager Role Swap Code
            CheckpointManager.ResetCheckpoints();
            //--Audio
            //--UI
            UIManager.RoleSwapReset(newRunner, newChaser);
            AbilityManager.ResetAbilities();
        }

        internal IEnumerator ContinueManageRoleSwap(Player newRunner, Player newChaser) {
            yield return new WaitForSeconds(3);
            PlayerManager.ControlPlayerRoleSwap(newRunner, newChaser);          // Start Player Manager Role Swap Code
            RoadManager.ResetRoad(newRunner.RoadSpawnData);               // Start Road Manager Role Swap Code
            CheckpointManager.ResetCheckpoints();
            //--Audio
            //--UI
            UIManager.RoleSwapReset(newRunner, newChaser);
            AbilityManager.ResetAbilities();
        }

        public void ManageRoundOver() {
            //ScoreManager.UpdateScore();
            PlayerManager.ResetPlayersAfterRound();
            RoadManager.ResetRoad(PlayerManager.CurrentRunner.RoadSpawnData);
            CheckpointManager.ResetCheckpoints();
            //DynamicObjectManager.ResetObjects();
            //UI
            UIManager.RoundStartReset();
            AbilityManager.ResetAbilities();
            StartCoroutine(RoundManager.RoundStart());

        }
        private void ContinueManageRound() {
            //ScoreManager.UpdateScore();
            PlayerManager.ResetPlayersAfterRound();
            RoadManager.ResetRoad(PlayerManager.CurrentRunner.RoadSpawnData);
            CheckpointManager.ResetCheckpoints();
            //DynamicObjectManager.ResetObjects();
            //UI
            UIManager.RoundStartReset();
            AbilityManager.ResetAbilities();
            StartCoroutine(RoundManager.RoundStart());
        }
    }
}
