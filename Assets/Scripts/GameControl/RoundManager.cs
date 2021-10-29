using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag
{
    public class RoundManager : MonoBehaviour
    {
        [SerializeField] private float runnerStartWaitTime = 3;      // countdown time before runner can start
        [SerializeField] private float chaserStartWaitTime = 4;     // countdown timer before chaser can start
        [SerializeField] private float distanceToWin = 200;
        private bool checkDistance = false;


        public float DistanceToWin {
            get { return distanceToWin; }
            set { distanceToWin = value; }
        }

        public IEnumerator RoundStart() {
            GameManager.Instance.PlayerManager.DisableCars();           // all Cars Disabled
            //StartCoroutine(GameManager.Instance.UIManager.DisplayCount(roundStartWaitTime, chaserStartWaitTime)); // UIManager Displays Count
            yield return new WaitForSeconds(runnerStartWaitTime);        // wait till runner can start
            GameManager.Instance.PlayerManager.EnableRunner();          // runner Enabled
            //print("Runner Enabled");
            checkDistance = true;
            yield return new WaitForSeconds(chaserStartWaitTime);        // wait till chaser can start
            //print("Chaser Enabled");
            GameManager.Instance.PlayerManager.EnableChasers();          // chaser enabled
        }
        [SerializeField] float distanceTravelled;
        private void Update() {
            distanceTravelled = GameManager.Instance.RoadManager.Distance.TotalDistance;
            
            if (GameManager.Instance.RoadManager.Distance.TotalDistance >= distanceToWin && !CheckIfShouldStartOvertime() && checkDistance) {
                checkDistance = false;
                RoundWin();
                print("Runner Wins");
            }
        }

        private bool CheckIfShouldStartOvertime() {
            return false;
        }

        public void RoundWin() {
            GameManager.Instance.ManageRoundOver();
        }
    }
}
