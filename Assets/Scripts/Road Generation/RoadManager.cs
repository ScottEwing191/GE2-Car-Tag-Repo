using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Checkpoints;


namespace CarTag.Road
{
    public class RoadManager : MonoBehaviour
    {
        [SerializeField] private CheckpointManager checkpointManager;
        public RoadGenerator roadGenerator { get; set; }
        [SerializeField] private RoadSpawnData roadSpawnData;
        public RoadSpawnData RoadSpawnData {get { return roadSpawnData; }}


        private void Awake() {
            roadGenerator = GetComponentInChildren<RoadGenerator>();
            checkpointManager = FindObjectOfType<CheckpointManager>();
        }

       private void Update() {
            if (roadGenerator.TryGenerateRoad()) {
                if (checkpointManager != null) {
                   //checkpointManager.CheckpointSpawner.TrySpawnCheckpoint(RoadSpawnData.transform);
                   checkpointManager.CheckpointSpawner.TrySpawnCheckpoint(roadSpawnData.transform);

                }
            }
        }
    }
}
