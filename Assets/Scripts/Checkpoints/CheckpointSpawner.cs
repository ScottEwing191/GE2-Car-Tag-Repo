using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CarTag.Checkpoints
{
    public class CheckpointSpawner : MonoBehaviour
    {
        private CheckpointManager checkpointManager;
        //[AssetsOnly]
        [SerializeField] private GameObject checkpointPrefab;

        Transform spawnTransform;

        private void Awake() {
            checkpointManager = GetComponent<CheckpointManager>();
        }


        internal bool TrySpawnCheckpoint(Transform spawnTransform) {
            // Instantiate Checkpoint
            GameObject newCP = Instantiate(checkpointPrefab,
                spawnTransform.position,
                //position,
                spawnTransform.rotation);
                //rotation);
                //checkpointManager.gameObject.transform);    // parent
            //checkpointManager.Checkpoints.Enqueue(newCP.GetComponent<Checkpoint>());    // Add new Checkpoint to queue
            return true;
        }

    }
}
