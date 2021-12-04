using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Checkpoints;
using System;
using Dreamteck.Splines;
using CarTag.PlayerSpace;

namespace CarTag.Road {
    public class RoadManager : MonoBehaviour {
        [SerializeField] private CheckpointManager checkpointManager;

        //private bool isResetting = false;       // dont try and place a point on the spline for the fram that the road is reset

        //--Auto implemented properties
        public RoadGenerator RoadGenerator { get; set; }
        public PlayerManager PlayerManager { get; set; }
        //public RoadSpawnData RoadSpawnData { get; set; }
        public bool DoFixedUpdate { get; set; }                 // controlled by Round Manager True when round starts False when runner reaches target distance

        //-- Private
        private Distance distance = new Distance();
        private RoadRemoval roadRemoval = new RoadRemoval();
        private bool InitialSetupDone = false;


        //--Properties
        public Distance Distance { get { return distance; } }


        public void InitialSetup(RoadSpawnData initialRoadSpawnData) {
            PlayerManager = GameManager.Instance.PlayerManager;
            RoadGenerator = GetComponentInChildren<RoadGenerator>();
            checkpointManager = FindObjectOfType<CheckpointManager>();
            //RoadSpawnData = initialRoadSpawnData;                           // Setup Road Generation
            RoadGenerator.InitialSetup(initialRoadSpawnData);
            InitialSetupDone = true;

        }



        private void Update() {
            //RoadSpawnData = GameManager.Instance.PlayerManager.CurrentRunner.RoadSpawnData;
            print("RSD" + PlayerManager.CurrentRunner.RoadSpawnData.transform.parent.name);
        }

        private void FixedUpdate() {

            if (!InitialSetupDone) {                        // make sure that fixed update does not run until initial setup has been done (Added now that inital setup
                return;                                     // does ot get done on Start() anymore
            }
            if (DoFixedUpdate) {
                RoadSpawnData roadSpawnData = PlayerManager.CurrentRunner.RoadSpawnData;
                if (RoadGenerator.TryGenerateRoad()) {      // if the road was succesfully generated (i.e new point added to spline)
                    if (checkpointManager != null) {
                        
                        //checkpointManager.StartCheckpointSpawn(RoadSpawnData.Position, RoadSpawnData.transform.rotation);    // tell checkpoint system to try and spawn a checkpoint
                        checkpointManager.StartCheckpointSpawn(roadSpawnData.Position, roadSpawnData.transform.rotation);    // tell checkpoint system to try and spawn a checkpoint

                    }
                    Vector3 newestPointInSpline = RoadGenerator.SplineComputer.GetPoint(RoadGenerator.SplineComputer.pointCount - 1).position;
                    distance.SetNewPointAddedDistance(newestPointInSpline);
                }
                else {      // no new points were added to spline
                            //--uses the same values as used in road generator but will not add distance while car is in air.
                            //--uses different values as used in road generator so may be less acuurate but add distance while car is in air
                    
                    //distance.SetNoPointAddedDistance(RoadSpawnData.transform.position);
                    distance.SetNoPointAddedDistance(roadSpawnData.transform.position);

                }
            }
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
            StartCoroutine(RoleSwapRoutine());
            //RoadSpawnData = newRoadSpawnData;    // Update RoadManager RoadSpawnData
            distance.ResetDistanceTravelled();
            SplinePoint[] emptySplinePoints = new SplinePoint[0];
            RoadGenerator.SplineComputer.SetPoints(emptySplinePoints);
            //isResetting = true;
        }

        //--Don't try and update the road for a second after the round resets or role is swapped
        private IEnumerator RoleSwapRoutine() {
            DoFixedUpdate = false;
            yield return new WaitForSeconds(1);
            DoFixedUpdate = true;
        }
    }
}
