using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using Sirenix.OdinInspector;

namespace CarTag.Road
{
    public class RoadGenerator : MonoBehaviour
    {
        //[SerializeField] private Transform roadSpawnPoint;
        [SerializeField] private RoadSpawnData roadSpawnData;
        [SerializeField] private SplineComputer splineComputer;

        [SerializeField] private float maxDisplacementBetweenControlPoints = 3;
        [SerializeField] private float displacementSinceLastSplinePoint;
        [SerializeField] float pointSize = 1;

        [SerializeField] private Spline.Type splineType;


        private Vector3 currentPosition;

        private void Awake() {
            currentPosition = roadSpawnData.Position;
        }

        private void LateUpdate() {
            //TryGenerateRoad();
        }

        // Return true if road is generated
        internal bool TryGenerateRoad() {
            splineComputer.type = splineType;
            currentPosition = roadSpawnData.Position;

            // Car quicly goes off ground then back on when going onto ramp. This causes two points to be added quickly 
            /*if (roadSpawnData.GroundedThisFrame || roadSpawnData.OffGroundThisFrame) {
                AddControlPoint();
            }*/
            if (IsDisplacementTravelled()) {
                AddControlPoint();
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
        
        private void AddControlPoint() {
            SplinePoint splinePoint = new SplinePoint(roadSpawnData.Position, roadSpawnData.Position, roadSpawnData.Normal, 0.5f, Color.red);
            splinePoint.size = pointSize;
            splineComputer.SetPoint(splineComputer.pointCount, splinePoint);
        }
    }
}
