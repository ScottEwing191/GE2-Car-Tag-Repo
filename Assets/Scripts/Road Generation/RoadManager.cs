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
        private Distance distance = new Distance();
        private RoadRemoval roadRemoval = new RoadRemoval();

        public void InitialSetup(RoadSpawnData initialRoadSpawnData) {
            RoadGenerator = GetComponentInChildren<RoadGenerator>();
            checkpointManager = FindObjectOfType<CheckpointManager>();
            RoadSpawnData = initialRoadSpawnData;                           // Setup Road Generation
            RoadGenerator.InitialSetup(initialRoadSpawnData);

        }
       private void FixedUpdate() {
            if (RoadGenerator.TryGenerateRoad()) {      // if the road was succesfully generated (i.e new point added to spline)
                if (checkpointManager != null) {
                    //checkpointManager.StartCheckpointSpawn(RoadSpawnData.transform);    // tell checkpoint system to try and spawn a checkpoint
                    checkpointManager.StartCheckpointSpawn(RoadSpawnData.Position, RoadSpawnData.transform.rotation);    // tell checkpoint system to try and spawn a checkpoint
                }
                Vector3 newestPointInSpline = RoadGenerator.SplineComputer.GetPoint(RoadGenerator.SplineComputer.pointCount - 1).position;
                distance.SetNewPointAddedDistance(newestPointInSpline);
            }
            else {      // no new points were added to spline
                //--uses the same values as used in road generator but will not add distance while car is in air.
                //distance.SetNoPointAddedDistance(RoadSpawnData.Position);
                //--uses different values as used in road generator so may be less acuurate but add distance while car is in air
                distance.SetNoPointAddedDistance(RoadSpawnData.transform.position);
            }
            //print("Distance: " + distance.TotalDistance);
        }

        public void RemoveRoad(Vector3 cpPosition) {
            RoadGenerator.SplineComputer.SetPoints(roadRemoval.RemovePoints(RoadGenerator.SplineComputer.GetPoints(), cpPosition));
        }

        public void ControlRoleSwap(RoadSpawnData newRoadSpawnData) {
            RoadSpawnData = newRoadSpawnData;    // Update RoadManager RoadSpawnData
            distance.ResetDistanceTravelled();
        }
    }
}
