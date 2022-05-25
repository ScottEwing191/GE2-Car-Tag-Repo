using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using CarTag.UI;
using CarTag.PlayerSpace;

namespace CarTag.Checkpoints {
    public class CheckpointManager : MonoBehaviour {
        [ShowInInspector] private List<Queue<Checkpoint>> checkpointQueues = new List<Queue<Checkpoint>>();
        [SerializeField] private int visibleCheckpoints = 5;
        [SerializeField] private bool spawnCheckpoints = true;

        // Auto-implemented Properties
        public UIManager UIManager { get; private set; }
        public PlayerManager PlayerManager { get; private set; }
        public CheckpointSpawner CheckpointSpawner { get; private set; }
        public CheckpointVisibility CheckpointVisibility { get; set; }
        public CheckpointReached CheckpointReached { get; set; }
        public List<PlayerCheckpointsController> PlayerCheckpointsControllers { get; set; }


        // Properties
        public List<Queue<Checkpoint>> CheckpointQueues { get { return checkpointQueues; } }

        private void Awake() {
            CheckpointSpawner = GetComponent<CheckpointSpawner>();
            CheckpointVisibility = new CheckpointVisibility(this, visibleCheckpoints);
            CheckpointReached = new CheckpointReached(this);
            PlayerCheckpointsControllers = new List<PlayerCheckpointsController>();
        }

        private void OnEnable() {
            GameEvents.onRoleSwapNullary += ResetCheckpoints;
        }

        private void OnDisable() {
            GameEvents.onRoleSwapNullary -= ResetCheckpoints;
        }

        public void InitialSetup(int numberOfQueues) {
            UIManager = GameManager.Instance.UIManager;
            PlayerManager = GameManager.Instance.PlayerManager;
            for (int i = 0; i < numberOfQueues; i++) {
                checkpointQueues.Add(new Queue<Checkpoint>());
            }
            //--Set up Player Checkpoints Controller Lists
            foreach (Player player in PlayerManager.Players) {
                PlayerCheckpointsControllers.Add(player.GetComponentInChildren<PlayerCheckpointsController>());
            }
            foreach (PlayerCheckpointsController p in PlayerCheckpointsControllers) {
                p.InitialSetup();
            }
        }

        /// <summary>
        /// Takes in the transform where a checkpoint it to be spawned. Tells Checkpoint Spawner to try and spawn checkpoint
        /// if sucessful, tells CheckpointVisibility to set the visibility of the checkpoint appropriately
        /// </summary>
        internal void StartCheckpointSpawn(Vector3 cpPosition, Quaternion cpRotation) {
            if (!spawnCheckpoints) {
                return;
            }
            // get the index of the current runner so that the checkpoint system knows which Queue to avoid adding checkpoints to
            int currentRunnerIndex = PlayerManager.CurrentRunner.PlayerListIndex;
            var newCPScript = CheckpointSpawner.TrySpawnCheckpoint(cpPosition, cpRotation, currentRunnerIndex);
            if (newCPScript != null) {
                CheckpointVisibility.SetNewCheckpointVisibility(newCPScript, currentRunnerIndex);
                //--Set CP spawn UI
                UIManager.ManageCheckpointSpawnUI(CheckpointSpawner.PointsSinceLastCP, CheckpointSpawner.SpawnFrequency, true, runnerCpAhead());   
            }
            else {
                //--Set CP spawn UI (no spawn)
                UIManager.ManageCheckpointSpawnUI(CheckpointSpawner.PointsSinceLastCP, CheckpointSpawner.SpawnFrequency, false);
            }

        }

        
        /// <summary>
        /// Called from checkpoint OnTriggerEnter when player collides with Checkpoint
        /// Tells the CheckpointReached script to determine if the correct checkpoint was collided with
        /// </summary>
        /// <param name="checkpoint">The Checkpoint Which was collided with</param>
        /// <param name="player">The player which collided with the checkpoint</param>
        public void HandleCheckpointReached(Checkpoint checkpoint, Player player) {
            CheckpointReached.DoCheckpointReached(checkpoint, player);
            //--Update Checkpoint UI 
            UIManager.ManageCheckpointReachedUI(runnerCpAhead());
        }

        /// <summary>
        /// Finds the checkpoints queue which contains the most checkpoints (i.e all the checkpoints still in the scene) and destroys all
        /// the checkpoint game objects containing the checpoints scripts in the queue.
        /// Also clears all checkpoint queues
        /// </summary>
        internal void ResetCheckpoints() {
            CheckpointSpawner.PointsSinceLastCP = 0;    // reset when the next checkpointwill spawn
            //--Find the Longest Checkpoint Queue
            int longestQueueIndex = 0;
            for (int i = 0; i < checkpointQueues.Count; i++) {
                if (checkpointQueues[i].Count > checkpointQueues[longestQueueIndex].Count) {
                    longestQueueIndex = i;
                }
            }
            //--Destroy the gameobjects that the checkpoint scripts in the queue are attached to 
            int count = checkpointQueues[longestQueueIndex].Count;
            for (int i = 0; i < count; i++) {
                Destroy(checkpointQueues[longestQueueIndex].Dequeue().gameObject);
            }

            //--Clear all the Checkpoint Queues
            for (int i = 0; i < checkpointQueues.Count; i++) {
                checkpointQueues[i].Clear();
            }
        }

        /// <summary>
        /// Returns the number of checkpoints left for the Chaser in the lead
        /// </summary>
        private int runnerCpAhead() {
            int shortest = int.MaxValue;
            for (int i = 0; i < checkpointQueues.Count; i++) {
                if (i != PlayerManager.CurrentRunner.PlayerListIndex) {     // if not the runners queue
                    shortest = Mathf.Min(shortest, checkpointQueues[i].Count);
                }
            }
            return shortest;
        }

        /// <summary>
        /// Update dates the guide arrow for each player
        /// </summary>
        /// Called when new checkpoint is spawned
        public void UpdateCheckpointGuides() {
            foreach (PlayerCheckpointsController controller in PlayerCheckpointsControllers) {
                controller.CheckpointGuide.UpdateGuide();
            }
        }

    }
}
