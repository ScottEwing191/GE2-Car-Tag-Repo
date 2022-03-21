using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.PlayerSpace;
using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;


namespace CarTag.Checkpoints {
    public class PlayerCheckpointsController : MonoBehaviour {
        public CheckpointManager CheckpointManager { get; set; }
        public Player ThisPlayer { get; set; }
        public CheckpointGuide CheckpointGuide { get; set; }
        public Queue<Checkpoint> CheckpointsQueue { get; private set; }



        public void InitialSetup() {
            CheckpointManager = GameManager.Instance.CheckpointManager;
            ThisPlayer = GetComponentInParent<Player>();
            CheckpointGuide = GetComponentInChildren<CheckpointGuide>();
            CheckpointGuide.InitialSetup();
            CheckpointsQueue = CheckpointManager.CheckpointQueues[ThisPlayer.PlayerListIndex];

        }

        
    }
}