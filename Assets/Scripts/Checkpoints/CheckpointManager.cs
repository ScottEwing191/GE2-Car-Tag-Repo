using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace CarTag.Checkpoints {
    public class CheckpointManager : MonoBehaviour {
        [ShowInInspector] private Queue<Checkpoint> checkpoints = new Queue<Checkpoint>();
        //[ShowInInspector] private List<Queue<Checkpoint>> listOfQueueOfCheckpoints = new List<Queue<Checkpoint>>();
        [SerializeField] private int visibleCheckpoints = 5;
        public Queue<Checkpoint> Checkpoints { get { return checkpoints; } }
        public CheckpointSpawner CheckpointSpawner { get; private set; }
        public CheckpointVisibility CheckpointVisibility { get; set; }
        public CheckpointReached CheckpointReached { get; set; }

        private void Awake() {
            CheckpointSpawner = GetComponent<CheckpointSpawner>();
            CheckpointVisibility = new CheckpointVisibility(this, visibleCheckpoints);
            CheckpointReached = new CheckpointReached(this);
        }

       
        /// <summary>
        /// Takes in the transform where a checkpoint it to be spawned. Tells Checkpoint Spawner to try and spawn checkpoint
        /// if sucessful, tells CheckpointVisibility to set the visibility of the checkpoint appropriately
        /// </summary>
        internal void StartCheckpointSpawn(Transform roadSpawnDataTransform) {
            var newCPScript = CheckpointSpawner.TrySpawnCheckpoint(roadSpawnDataTransform);
            if (newCPScript != null) {
                CheckpointVisibility.SetNewCheckpointVisibility(newCPScript);
            }
        }
    }
}
