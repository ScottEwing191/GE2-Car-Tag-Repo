using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.PlayerSpace;
using CarTag.UI;
using CarTag.Road;

namespace CarTag
{
    public class RoundManager : MonoBehaviour
    {
        //--Serialized fields
        [SerializeField] private float runnerStartWaitTime = 3;      // countdown time before runner can start
        [SerializeField] private float chaserStartWaitTime = 4;     // countdown timer before chaser can start
        [SerializeField] private float distanceToWin = 200;
        private bool checkDistance = false;
        private bool initialSetupDone = false;

        //--Auto-Implemented Properties
        public UIManager UIManager { get; private set; }
        public PlayerManager PlayerManager { get; private set; }
        public RoadManager RoadManager { get; set; }

        //--Properties
        public float DistanceToWin {
            get { return distanceToWin; }
            set { distanceToWin = value; }
        }

        public void InitalSetup() {
            PlayerManager = GameManager.Instance.PlayerManager;
            UIManager = GameManager.Instance.UIManager;
            RoadManager = GameManager.Instance.RoadManager;
            initialSetupDone = true;
        }

        public IEnumerator RoundStart() {
            PlayerManager.DisableCars();                                    // all cars disabled
            UIManager.StartRunnerCountdown(runnerStartWaitTime);
            UIManager.StartChaserCountdown(runnerStartWaitTime + chaserStartWaitTime);
            UIManager.UpdateRunnerDistanceTracker(0, DistanceToWin);        // set distance tracker UI to 0 on round tart
            yield return new WaitForSeconds(runnerStartWaitTime);           // wait till runner can start
            PlayerManager.EnableRunner();                                   // runner Enabled
            checkDistance = true;
            yield return new WaitForSeconds(chaserStartWaitTime);           // wait till chaser can start
            PlayerManager.EnableChasers();                                  // chaser enabled
            UIManager.SetupChaserCheckpointTrackers();
            
        }
        [SerializeField] float distanceTravelled;
        private void Update() {
            if (initialSetupDone) {
                distanceTravelled = RoadManager.Distance.TotalDistance;
                UIManager.UpdateRunnerDistanceTracker(distanceTravelled, DistanceToWin);  // update Runner Distance Tracker UI
                if (RoadManager.Distance.TotalDistance >= distanceToWin && !CheckIfShouldStartOvertime() && checkDistance) {
                    checkDistance = false;
                    RoundWin();
                    print("Runner Wins");
                } 
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
