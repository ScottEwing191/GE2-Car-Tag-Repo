using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Checkpoints;


namespace CarTag.Road
{
    public class RoadManager : MonoBehaviour
    {
        [SerializeField] private CheckpointManager checkpointManager;
        public RoadGenerator RoadGenerator { get; set; }
        public RoadSpawnData RoadSpawnData { get; set; }

        public void InitialSetup(RoadSpawnData initialRoadSpawnData) {
            RoadGenerator = GetComponentInChildren<RoadGenerator>();
            checkpointManager = FindObjectOfType<CheckpointManager>();
            RoadSpawnData = initialRoadSpawnData;                           // Setup Road Generation
            RoadGenerator.InitialSetup(initialRoadSpawnData);
        }
       private void Update() {
            if (RoadGenerator.TryGenerateRoad()) {      // if the road was succesfully generated
                if (checkpointManager != null) {
                    checkpointManager.StartCheckpointSpawn(RoadSpawnData.transform);    // tell checkpoint system to try and spawn a checkpoint
                }
            }
        }
    }
}
