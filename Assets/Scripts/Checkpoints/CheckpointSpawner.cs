using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CarTag.Checkpoints {
    public class CheckpointSpawner : MonoBehaviour {
        public Action OnCheckpointSpawned = delegate { };
        [AssetsOnly]
        [SerializeField] private GameObject checkpointPrefab;
        [SerializeField] private float spawnFrequency = 10;         // the checkpoint will spawn every 10th point on the spline

        private CheckpointManager checkpointManager;
        //--Auto-Implemented Properties
        public float PointsSinceLastCP { get; set; }                       // how many points have passed since the last checkpoint 

        //--Properties
        public float SpawnFrequency { get { return spawnFrequency; } }
        private void Awake() {
            checkpointManager = GetComponent<CheckpointManager>();
        }

        /// <summary>
        /// Checks if a checkpoint is able spawn and then spawns it
        /// </summary>
        /// <param name="spawnTransform"></param>
        /// <param name="runnerCheckpointListIndex">This is the index in the List<Queue<Checkpoint>> of the runner queue. Checkpoints do not need to be added here</param>
        /// <returns>Returns new chekpoint script or null if no checkpoint was spawned</returns>
        
        internal Checkpoint TrySpawnCheckpoint(Vector3 cpPosition, Quaternion cpRotation, int runnerCheckpointListIndex) {
            //bool canSpawn = CanCheckpointSpawn();
            if (CanCheckpointSpawn()) {
                var newCheckpoint = SpawnCheckpoint(cpPosition, cpRotation, runnerCheckpointListIndex);
                checkpointManager.UpdateCheckpointGuides(); // update the checkpoint guide (only needed if the guide was previously pointing at the Runner )
                OnCheckpointSpawned?.Invoke();
                return newCheckpoint;
            }
            else {
                return null;
            }
        }

        private bool CanCheckpointSpawn() {
            PointsSinceLastCP++;
            if (PointsSinceLastCP % spawnFrequency == 0) {
                PointsSinceLastCP = 0;
                return true;
            }
            return false;
        }

        int cpName = 0;
        private Checkpoint SpawnCheckpoint(Vector3 position, Quaternion rotation, int runnerCheckpointListIndex) {
        // Instantiate Checkpoint Game object which will be used by each car
        GameObject newCP = Instantiate(checkpointPrefab,
                position,
                rotation,
                checkpointManager.gameObject.transform);    // parent
            Checkpoint newCheckpoint = newCP.GetComponent<Checkpoint>();              // Get one the checkpoint which will be added to each valid queue
            
            for (int i = 0; i < checkpointManager.CheckpointQueues.Count; i++) {
                if (i == runnerCheckpointListIndex)                                 // Dont add checkpoint to runner queue
                    continue;
            checkpointManager.CheckpointQueues[i].Enqueue(newCheckpoint);     // Add new Checkpoint to queue
            }
            cpName++;
            newCP.name = "Checkpoint " + cpName.ToString();
            return newCheckpoint;
        }
    }
}
