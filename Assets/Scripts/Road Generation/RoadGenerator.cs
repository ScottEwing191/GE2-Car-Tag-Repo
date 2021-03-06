using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using Sirenix.OdinInspector;

namespace CarTag.Road
{
    public class RoadGenerator : MonoBehaviour
    {
        [SerializeField] private SplineComputer splineComputer;
        [SerializeField] private float maxDisplacementBetweenControlPoints = 4;
        [SerializeField] float pointSize = 1;
        [SerializeField] private Spline.Type splineType;

        private float displacementSinceLastSplinePoint;
        private RoadManager RoadManager;
        private Vector3 currentPosition;

        //Properties
        public SplineComputer SplineComputer {
            get { return splineComputer; }
            set { splineComputer = value; }
        }


        private void Update() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.M)) {
                print("Local:" + splineComputer.GetPoint(9, SplineComputer.Space.Local).position);
                print("World:" + splineComputer.GetPoint(9, SplineComputer.Space.World).position);
            }
        }

        public void InitialSetup(RoadSpawnData data) {
            RoadManager = GetComponentInParent<RoadManager>();
            //currentPosition = roadManager.RoadSpawnData.Position;
            currentPosition = RoadManager.PlayerManager.CurrentRunner.RoadSpawnData.Position;
            splineComputer.space = SplineComputer.Space.World;
        }

        // Return true if road is generated
        internal bool TryGenerateRoad() {
            splineComputer.type = splineType;
            //Debug.Break();
            //currentPosition = RoadManager.RoadSpawnData.Position;
            currentPosition = RoadManager.PlayerManager.CurrentRunner.RoadSpawnData.Position;


            // Car quicly goes off ground then back on when going onto ramp. This causes two points to be added quickly 
            /*if (roadManager.RoadSpawnData.GroundedThisFrame || roadManager.RoadSpawnData.OffGroundThisFrame) {
                AddControlPoint();
            }*/
            if (IsDisplacementTravelled()) {
                AddSplinePoint();
                return true;
            }
            return false;
        }

        private bool IsDisplacementTravelled() {
            Vector3 lastPoint = splineComputer.GetPoint(splineComputer.pointCount - 1).position;
            displacementSinceLastSplinePoint = Vector3.Distance(currentPosition, lastPoint);
            if (displacementSinceLastSplinePoint >= maxDisplacementBetweenControlPoints) {
                displacementSinceLastSplinePoint = 0;
                return true;
            }
            else return false;
        }
        
        private void AddSplinePoint() {
            RoadSpawnData data = RoadManager.PlayerManager.CurrentRunner.RoadSpawnData;
            SplinePoint splinePoint = new SplinePoint(data.Position, data.Position, data.Normal, 0.5f, Color.red);
            splinePoint.size = pointSize;
            splineComputer.SetPoint(splineComputer.pointCount, splinePoint);
        }
    }
}
