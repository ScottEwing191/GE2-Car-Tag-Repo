using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Player;
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
        }

        public void ChangeRoles() {

        }
    }
}
