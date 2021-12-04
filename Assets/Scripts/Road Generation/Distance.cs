using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CarTag.Road {
    /// <summary>
    /// Uses the Spline created by the road generator to work out the distance travelled by the car
    /// If the car is betweenpoints on the split it will add on the distance from the last spline to the total distance
    /// </summary>
    public class Distance {
        public float TotalDistance { get; set; }    // the distace poind by adding up the distance between all the points on the spline 
                                                    // including the points that get deltated
        private float pointsDistance;               // the cumulative distance between all the points added to the spline for this particular driver
        private float maxDistplacement = 0;         // the max displacement the car has travelled from the last spline point. 

        private Vector3 lastPointPos;               // the position of the last point placed on the pline
        private bool firstCall = true;              // is this the first time the distance has been calculated for the current runner

        /// <summary>
        /// Called if a point has been added to the spline
        /// </summary>
        public void SetNewPointAddedDistance(Vector3 thisPointPosition) {
            // If this is the first call then there is no last point position yet
            if (firstCall) {
                lastPointPos = thisPointPosition;
                firstCall = false;
                return;
            }
            pointsDistance += Vector3.Distance(lastPointPos, thisPointPosition);
            TotalDistance = pointsDistance;
            lastPointPos = thisPointPosition;
            maxDistplacement = 0;
        }

        public void SetNoPointAddedDistance(Vector3 currentPosition) {
            if (firstCall) {
                lastPointPos = currentPosition;
                firstCall = false;
                return;
            }
            float thisDisplacement = Vector3.Distance(lastPointPos, currentPosition);
            //--the total distance is only changed if the current displacement from the last point is greater than the max displacement from ...
            //...the last point. THis means the car can not roll back and forth on the spot and have the distance keep increasing
            if (thisDisplacement > maxDistplacement) {
                TotalDistance = pointsDistance + thisDisplacement;
                maxDistplacement = thisDisplacement;
            }
        }

        public void ResetDistanceTravelled() {

            TotalDistance = 0;
            pointsDistance = 0;
            maxDistplacement = 0;
            firstCall = true;
        }
        /*private IEnumerator ResetDistanceTravelledRoutine() {
            yield return new WaitForEndOfFrame();
            TotalDistance = 0;
            pointsDistance = 0;
            maxDistplacement = 0;
            firstCall = true;
        }*/
    }
}
