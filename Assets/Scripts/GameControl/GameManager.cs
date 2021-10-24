using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.PlayerSpace;
using CarTag.Road;
using CarTag.Checkpoints;

namespace CarTag
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public PlayerManager PlayerManager { get; set; }
        public RoadManager RoadManager { get; set; }
        public CheckpointManager CheckpointManager { get; set; }

        protected override void Awake() {
            base.Awake();
            //InitialSetup();
        }

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


            PlayerManager.InitialSetup();                                //Setup player Runners
            RoadManager.InitialSetup(PlayerManager.CurrentRunner.RoadSpawnData);
            CheckpointManager.SetupQueues(PlayerManager.Players.Count);
        }


        /// <summary>
        /// This is called when the chaser catches the runner. It Initiate the Role Swap code in various classes in the game 
        /// </summary>
        /// <param name="newRunner">The Player Script on the new runner (old chaser)</param>
        /// <param name="newChaser">The Player Script on the new chaser (old runner)</param>
        internal void ManageRoleSwap(Player newRunner, Player newChaser) {
            PlayerManager.ControlPlayerRoleSwap(newRunner, newChaser);          // Start Player Manager Role Swap Code
            RoadManager.ControlRoleSwap(newRunner.RoadSpawnData);               // Start Road Manager Role Swap Code
            //--Audio
            //--UI
        }
    }
}
