using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Checkpoints;

namespace CarTag.Road
{
    public class RoadManager : MonoBehaviour
    {
        CheckpointManager checkpointManager;
        public RoadGenerator roadGenerator { get; set; }
        public RoadSpawnData roadSpawnData { get; set; }

        private void Awake() {
            roadGenerator = GetComponentInChildren<RoadGenerator>();
            checkpointManager = FindObjectOfType<CheckpointManager>();
        }

       private void Update() {
            if (roadGenerator.TryGenerateRoad();) {
                if (checkpointManager != null) {
                    checkpointManager.CheckpointSpawner.TrySpawnCheckpoint()
                }
            }
        }
    }
}
