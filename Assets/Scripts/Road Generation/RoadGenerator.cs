using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using Sirenix.OdinInspector;

namespace CarTag.RoadGeneration
{
    public class RoadGenerator : MonoBehaviour
    {
        //[SerializeField] private Transform roadSpawnPoint;
        [SerializeField] private RoadSpawnData roadSpawnData;
        [SerializeField] private SplineComputer splineComputer;
        [SerializeField] private float maxDistanceBetweenControlPoints = 3;
        [SerializeField] private float distanceSinceLastSplinePoints;
        [ShowInInspector] private float distanceSinceLastControlPoint;
        [SerializeField] float pointSize = 1;

        [SerializeField] private Spline.Type splineType;

        private Vector3 lastPosition;
        private Vector3 currentPosition;
        private float distanceInLastFrame;

        private void Awake() {
            currentPosition = roadSpawnData.Position;
            lastPosition = currentPosition;
        }

        private void LateUpdate() {
            splineComputer.type = splineType;
            currentPosition = roadSpawnData.Position;
            distanceInLastFrame = Vector3.Distance(currentPosition, lastPosition);
            distanceSinceLastControlPoint += distanceInLastFrame;
            
            // Car quicly goes off ground then back on when going onto ramp. This causes two points to be added quickly 
            /*if (roadSpawnData.GroundedThisFrame || roadSpawnData.OffGroundThisFrame) {
                AddControlPoint();
            }*/
            if (distanceSinceLastControlPoint >= maxDistanceBetweenControlPoints) {
                AddControlPoint();
            }
            lastPosition = currentPosition;
        }

        private void AddControlPoint() {
            distanceSinceLastControlPoint = 0;
            SplinePoint splinePoint = new SplinePoint(roadSpawnData.Position, roadSpawnData.Position, roadSpawnData.Normal, 0.5f, Color.red);
            splinePoint.size = pointSize;
            splineComputer.SetPoint(splineComputer.pointCount, splinePoint);
        }

        private void AddCheckpoint() {
            
        }

    }
}
