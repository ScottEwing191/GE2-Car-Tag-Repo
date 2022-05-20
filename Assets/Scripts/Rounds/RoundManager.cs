using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.PlayerSpace;
using CarTag.UI;
using CarTag.Road;

namespace CarTag.Rounds {
    public class RoundManager : MonoBehaviour {
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
        public bool IsRoundRunning { get; private set; }
        //--Private 
        private List<PlayerRoundController> playerRoundControllers = new List<PlayerRoundController>();

        //--Properties
        public float DistanceToWin {
            get { return distanceToWin; }
            set { distanceToWin = value; }
        }

        public List<PlayerRoundController> PlayerRoundControllers { get { return playerRoundControllers; } }
        //=== JUST REQUIRED FOR TELEMETRY ===
        public float ChaserStartWaitTime {
            get { return chaserStartWaitTime; }
            set { chaserStartWaitTime = value; }
        }

        public void InitalSetup() {
            PlayerManager = GameManager.Instance.PlayerManager;
            UIManager = GameManager.Instance.UIManager;
            RoadManager = GameManager.Instance.RoadManager;
            //--Setup Player Round Controllers
            for (int i = 0; i < PlayerManager.Players.Count; i++) {
                playerRoundControllers.Add(PlayerManager.Players[i].GetComponentInChildren<PlayerRoundController>());
                playerRoundControllers[i].InitialSetup();
            }

            initialSetupDone = true;
        }

        public IEnumerator RoundStart() {
            RoadManager.DoFixedUpdate = true;
            IsRoundRunning = true;
            Cursor.visible = false;
            PlayerManager.DisableCars();                                    // all cars disabled
            StartCoroutine(PlayerManager.CurrentRunner.PlayerCollision.TurnOffCollisionWithDelay(0));       //Turn off Car Collision for the Runner
            UIManager.StartRunnerCountdown(runnerStartWaitTime);
            UIManager.StartChaserCountdown(runnerStartWaitTime + chaserStartWaitTime);
            UIManager.UpdateRunnerDistanceTracker(0, DistanceToWin);        // set distance tracker UI to 0 on round tart
            yield return new WaitForSeconds(runnerStartWaitTime);           // wait till runner can start
            PlayerManager.EnableRunner();                                   // runner Enabled
            checkDistance = true;
            yield return new WaitForSeconds(chaserStartWaitTime);           // wait till chaser can start
            PlayerManager.EnableChasers();                                  // chaser enabled
            StartCoroutine(PlayerManager.CurrentRunner.PlayerCollision.TurnOnCarCollision(0));       //Turn On Car Collision for the Runner
            UIManager.SetupChaserCheckpointTrackers();

        }
        [SerializeField] float distanceTravelled;
        private void Update() {
            distanceTravelled = RoadManager.Distance.TotalDistance;
            UIManager.UpdateRunnerDistanceTracker(distanceTravelled, DistanceToWin);  // update Runner Distance Tracker UI
            if (RoadManager.Distance.TotalDistance >= distanceToWin && !CheckIfShouldStartOvertime() && checkDistance) {
                /*checkDistance = false;
                RoadManager.DoFixedUpdate = false;
                IsRoundRunning = false;*/
                RoundWin();
            }
        }

        private bool CheckIfShouldStartOvertime() {
            return false;
        }

        public void RoundWin() {
            checkDistance = false;
            RoadManager.DoFixedUpdate = false;
            IsRoundRunning = false;
            GameManager.Instance.ManageRoundOver();
        }
    }
}
