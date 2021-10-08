using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CarTag.Checkpoints {
    public class CheckpointSpawner : MonoBehaviour {
        //[AssetsOnly]
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
        /// <returns>Returns new chekpoint script or null if no checkpoint was spawned</returns>
        internal Checkpoint TrySpawnCheckpoint(Transform spawnTransform) {
            bool canSpawn = CanCheckpointSpawn(spawnTransform);
            if (canSpawn) {
                var newCPScript = SpawnCheckpoint(spawnTransform);
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
        int testName = 0;
        /// <summary>
        /// instantiates a new checkpoint and return checkpoint skript on that new gameobject
        /// </summary>
        private Checkpoint SpawnCheckpoint(Transform spawnTransform) {
            // Instantiate Checkpoint
            GameObject newCP = Instantiate(checkpointPrefab,
                spawnTransform.position,
                spawnTransform.rotation,
                checkpointManager.gameObject.transform);    // parent
            Checkpoint newCPScript= newCP.GetComponent<Checkpoint>();
            checkpointManager.Checkpoints.Enqueue(newCPScript);    // Add new Checkpoint to queue
            testName++;
            newCP.name = "Checkpoint " + testName.ToString();
            return newCPScript;
        }
    }
}
