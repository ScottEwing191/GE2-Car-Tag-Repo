using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace CarTag.Checkpoints {
    public class CheckpointManager : MonoBehaviour {
        [ShowInInspector] private List<Queue<Checkpoint>> checkpointQueues = new List<Queue<Checkpoint>>();
        [SerializeField] private int visibleCheckpoints = 5;
        
        // Auto-implemented Properties
        public CheckpointSpawner CheckpointSpawner { get; private set; }
        public CheckpointVisibility CheckpointVisibility { get; set; }
        public CheckpointReached CheckpointReached { get; set; }

        // Properties
        public List<Queue<Checkpoint>> CheckpointQueues { get { return checkpointQueues; } }

        private void Awake() {
            CheckpointSpawner = GetComponent<CheckpointSpawner>();
            CheckpointVisibility = new CheckpointVisibility(this, visibleCheckpoints);
            CheckpointReached = new CheckpointReached(this);
        }

        public void SetupQueues(int numberOfQueues) {
            for (int i = 0; i < numberOfQueues; i++) {
                checkpointQueues.Add(new Queue<Checkpoint>());
            }
        }

        /// <summary>
        /// Takes in the transform where a checkpoint it to be spawned. Tells Checkpoint Spawner to try and spawn checkpoint
        /// if sucessful, tells CheckpointVisibility to set the visibility of the checkpoint appropriately
        /// </summary>
        //-- OLD --internal void StartCheckpointSpawn(Transform roadSpawnDataTransform) {
        internal void StartCheckpointSpawn(Vector3 cpPosition, Quaternion cpRotation) {

            // get the index of the current runner so that the checkpoint system knows which Queue to avoid adding checkpoints to
            int currentRunnerIndex = GameManager.Instance.PlayerManager.CurrentRunner.PlayerListIndex;      
            //-- OLD --var newCPScript = CheckpointSpawner.TrySpawnCheckpoint(roadSpawnDataTransform, currentRunnerIndex);
            var newCPScript = CheckpointSpawner.TrySpawnCheckpoint(cpPosition, cpRotation, currentRunnerIndex);
            if (newCPScript != null) {
                CheckpointVisibility.SetNewCheckpointVisibility(newCPScript, currentRunnerIndex);

            }
        }
    }
}
