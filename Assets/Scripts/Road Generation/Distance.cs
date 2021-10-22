using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CarTag.Road
{
    /// <summary>
    /// Uses the Spline created by the road generator to work out the distance travelled by the car
    /// If the car is betweenpoints on the split it will add on the distance from the last spline to the total distance
    /// </summary>
    public class Distance : MonoBehaviour
    {
        public float TotalDistanceTravelled { get; set; }   // the distace poind by adding up the distance between all the points on the spline 
                                                            // including the points that get deltated
        private float pointsDistanceTravelled;
        private float pointCoint;

        public void ResetDistanceTravelled() {

        }

        //if point added
    }
}
