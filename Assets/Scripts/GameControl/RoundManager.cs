using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag
{
    public class RoundManager : MonoBehaviour
    {
        private float distanceToWin;

        public float DistanceToWin {
            get { return distanceToWin; }
            set { distanceToWin = value; }
        }

        public IEnumerator RoundStart() {
            yield return null;
            //--All Cars Disabled
            //--Countdown to start
            //--    UIManager Displays Count
            //--Runner Enabled
            //--Countdown to chaser enabled
            //--    UIManager Displays Count
            //--Chaser enabled
        }

        private void Update() {
            
        }

        public void RoundWin() {

        }
    }
}
