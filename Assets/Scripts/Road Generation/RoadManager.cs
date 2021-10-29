using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Checkpoints;
using System;
using Dreamteck.Splines;

namespace CarTag.Road
{
    public class RoadManager : MonoBehaviour
    {
        [SerializeField] private CheckpointManager checkpointManager;
        
        //--Auto implemented properties
        public RoadGenerator RoadGenerator { get; set; }
        public RoadSpawnData RoadSpawnData { get; set; }
        private Distance distance = new Distance();
        private RoadRemoval roadRemoval = new RoadRemoval();

        //--Properties
        public Distance Distance { get { return distance; } }


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

        /// <summary>
        /// Removes the point at the given position from the spline. And removes all points from the spline that came before the given point
        /// </summary>
        /// <param name="cpPosition"></param>
        public void RemoveRoad(Vector3 cpPosition) {
            SplinePoint[] newSplinePoints = roadRemoval.RemovePoints(RoadGenerator.SplineComputer.GetPoints(), cpPosition);
            RoadGenerator.SplineComputer.SetPoints(newSplinePoints);
        }
        /// <summary>
        /// Removes all points from the spline, Sets the new RoadSpawnData and Resets the distance travelled
        /// </summary>
        public void ResetRoad(RoadSpawnData newRoadSpawnData) {
            RoadSpawnData = newRoadSpawnData;    // Update RoadManager RoadSpawnData
            distance.ResetDistanceTravelled();
            SplinePoint[] emptySplinePoints = new SplinePoint[0];
            RoadGenerator.SplineComputer.SetPoints(emptySplinePoints);
        }

        
    }
}
