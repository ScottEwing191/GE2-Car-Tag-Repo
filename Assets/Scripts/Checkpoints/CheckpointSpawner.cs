using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CarTag.Checkpoints
{
    public class CheckpointSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject checkpointPrefab;
        [SerializeField] private float spawnFrequency = 10;        // the checkpoint will spawn every 4th point on the spline

        private float pointsSinceLastCP;                     // how many points have passed since the last checkpoint 
        private CheckpointManager checkpointManager;
        //[AssetsOnly]

        

        private void Awake() {
            checkpointManager = GetComponent<CheckpointManager>();
        }


        internal bool TrySpawnCheckpoint(Transform spawnTransform) {
            bool canSpawn = CanCheckpointSpawn(spawnTransform);
            if (canSpawn) {
                SpawnCheckpoint(spawnTransform);
                    return true;
            }
            else {
                return false;
            }
        }

        private bool CanCheckpointSpawn(Transform spawnTransform) {
            pointsSinceLastCP++;
            float mod = pointsSinceLastCP % spawnFrequency;
            print(mod);
            if (mod == 0) {
                pointsSinceLastCP = 0;
                return true;
            }
            return false;
        }
        int testName = 0;
        private void SpawnCheckpoint(Transform spawnTransform) {
            // Instantiate Checkpoint
            GameObject newCP = Instantiate(checkpointPrefab,
                spawnTransform.position,
                spawnTransform.rotation,
                checkpointManager.gameObject.transform);    // parent
            checkpointManager.Checkpoints.Enqueue(newCP.GetComponent<Checkpoint>());    // Add new Checkpoint to queue
            testName++;
            newCP.name = "Checkpoint " +testName.ToString();
        }

        /*private void Update() {
            if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame ) {
                checkpointManager.Checkpoints.Dequeue();
            }
        }*/

    }
}
