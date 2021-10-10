using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CarTag.Checkpoints {
    public class CheckpointSpawner : MonoBehaviour {
        [AssetsOnly]
        [SerializeField] private GameObject checkpointPrefab;
        [SerializeField] private float spawnFrequency = 10;        // the checkpoint will spawn every 4th point on the spline

        private float pointsSinceLastCP;                     // how many points have passed since the last checkpoint 
        private CheckpointManager checkpointManager;

        private void Awake() {
            checkpointManager = GetComponent<CheckpointManager>();
        }

        /// <summary>
        /// Checks if a checkpoint is able spawn and then spawns it
        /// </summary>
        /// <param name="spawnTransform"></param>
        /// <param name="runnerCheckpointListIndex">This is the index in the List<Queue<Checkpoint>> of the runner queue. Checkpoints do not need to be added here</param>
        /// <returns>Returns new chekpoint script or null if no checkpoint was spawned</returns>
        internal Checkpoint TrySpawnCheckpoint(Transform spawnTransform, int runnerCheckpointListIndex) {
            bool canSpawn = CanCheckpointSpawn(spawnTransform);
            if (canSpawn) {
                var newCPScript = SpawnCheckpoint(spawnTransform, runnerCheckpointListIndex);
                return newCPScript;
            }
            else {
                return null;
            }
        }

        private bool CanCheckpointSpawn(Transform spawnTransform) {
            pointsSinceLastCP++;
            if (pointsSinceLastCP % spawnFrequency == 0) {
                pointsSinceLastCP = 0;
                return true;
            }
            return false;
        }
        int cpName = 0;

        private Checkpoint SpawnCheckpoint(Transform spawnTransform, int runnerCheckpointListIndex) {
            // Instantiate Checkpoint Game object which will be used by each car
                GameObject newCP = Instantiate(checkpointPrefab,
                spawnTransform.position,
                spawnTransform.rotation,
                checkpointManager.gameObject.transform);    // parent
            Checkpoint newCPScript = newCP.GetComponent<Checkpoint>();              // Get one the checkpoint which will be added to each valid queue
            
            for (int i = 0; i < checkpointManager.CheckpointQueues.Count; i++) {
                if (i == runnerCheckpointListIndex)                                 // Dont add checkpoint to runner queue
                    continue;
            checkpointManager.CheckpointQueues[i].Enqueue(newCPScript);     // Add new Checkpoint to queue
            }
            cpName++;
            newCP.name = "Checkpoint " + cpName.ToString();
            return newCPScript;
        }
    }
}
