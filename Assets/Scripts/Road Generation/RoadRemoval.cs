using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

namespace CarTag.Road {
    public class RoadRemoval {

        /// <summary>
        /// Returns an array on SplinePoint's where all the points before the point passed in have been removed
        /// </summary>
        public SplinePoint[] RemovePoints(SplinePoint[] points, Vector3 splinePointPosition) {
            int cpPointIndex = -1;
            bool keepPoint = false;
            List<SplinePoint> pointsToKeep = new List<SplinePoint>();
            for (int i = 0; i < points.Length; i++) {
                
                if (points[i].position == splinePointPosition) {
                    cpPointIndex = i;
                    keepPoint = true;
                }
                if (keepPoint) {
                    pointsToKeep.Add(points[i]);
                }
            }
            return pointsToKeep.ToArray();
        }
    }
}
