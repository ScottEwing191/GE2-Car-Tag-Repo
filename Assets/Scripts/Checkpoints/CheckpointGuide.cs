using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.PlayerSpace;
using System;

//=============================================================
//
// CHeckpoint Guide is an arrow that will look at the players next target. Either a checkpoint or the runner
//
//=============================================================

namespace CarTag.Checkpoints {
    public class CheckpointGuide : MonoBehaviour {
        public Player ThisPlayer { get; set; }
        CheckpointManager CheckpointManager { get; set; }
        public PlayerCheckpointsController PlayerCheckpointsController { get; set; }


        private void Awake() {
            ThisPlayer = GetComponentInParent<Player>();
        }
        
        internal void InitialSetup() {
            CheckpointManager = ThisPlayer.PlayerManager.CheckpointManager;
            PlayerCheckpointsController = GetComponentInParent<PlayerCheckpointsController>();
        }

        private void OnEnable() {
            ThisPlayer.playerEnabledEvent += UpdateGuide;
        }
        private void OnDisable() {
            ThisPlayer.playerEnabledEvent -= UpdateGuide;
        }

        //The Checkpoint Guide is an arrow that points to the next checkpoint or the runner if all checkpoints have been reached
        public void UpdateGuide() {
            if (ThisPlayer.IsThisPlayerCurrentRunner()) { return; }
            if (CheckpointManager.CheckpointQueues[ThisPlayer.PlayerListIndex].Count > 0) {
                ThisPlayer.PlayerUIController.CheckpointGuideUI.Target = CheckpointManager.CheckpointQueues[ThisPlayer.PlayerListIndex].Peek().transform;
            }
            else {
                ThisPlayer.PlayerUIController.CheckpointGuideUI.Target = ThisPlayer.PlayerManager.CurrentRunner.RCC_CarController.transform;
            }
        }

    }
}
