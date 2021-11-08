using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.PlayerSpace;
using CarTag.UI;

namespace CarTag
{
    public class RoundManager : MonoBehaviour
    {
        //--Serialized fields
        [SerializeField] private float runnerStartWaitTime = 3;      // countdown time before runner can start
        [SerializeField] private float chaserStartWaitTime = 4;     // countdown timer before chaser can start
        [SerializeField] private float distanceToWin = 200;
        private bool checkDistance = false;

        //--Auto-Implemented Properties
        public UIManager UIManager { get; private set; }
        public PlayerManager PlayerManager { get; private set; }

        //--Properties
        public float DistanceToWin {
            get { return distanceToWin; }
            set { distanceToWin = value; }
        }

        public void InitalSetup() {
            PlayerManager = GameManager.Instance.PlayerManager;
            UIManager = GameManager.Instance.UIManager;
        }

        public IEnumerator RoundStart() {
            PlayerManager.DisableCars();           // all Cars Disabled
            UIManager.StartRunnerCountdown(runnerStartWaitTime);
            UIManager.StartChaserCountdown(runnerStartWaitTime + chaserStartWaitTime);
            yield return new WaitForSeconds(runnerStartWaitTime);       // wait till runner can start
            PlayerManager.EnableRunner();          // runner Enabled
            checkDistance = true;
            yield return new WaitForSeconds(chaserStartWaitTime);       // wait till chaser can start
            PlayerManager.EnableChasers();         // chaser enabled
            UIManager.SetupChaserCheckpointTrackers();
            
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
