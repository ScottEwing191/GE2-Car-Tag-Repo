using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CarTag.UI {
    public class PlayerUIController : MonoBehaviour {
        //--Serialized Fields
        [SerializeField] PlayerUIElements playerUI;
        [SerializeField] RunnerUIElements runnerUI;
        [SerializeField] ChaserUIElements chaserUI;

        private Player thisPlayer;          // used to gain acess to other controller script attached to the same player as this one
        
        //--Auto-Implemented Properties
        public ChaserCheckpointTracker ChaserCheckpointTracker { get; set; }
        public AbilityUI AbilityUI { get; set; }

        private void Awake() {
            ChaserCheckpointTracker = new ChaserCheckpointTracker(chaserUI.CheckpointTracker);
            AbilityUI = GetComponent<AbilityUI>();
            thisPlayer = GetComponentInParent<Player>();
        }

        public void InitialSetup() {
            AbilityUI.InitialSetup(playerUI.AbilityUIElements, thisPlayer.PlayerAbilityController.CurrentAbility.UsesLeft);
        } 
        //--Methods
        /// <summary>
        /// Start the countdown timer at the given time, When the timer reaches 0 display a message for the given time then hide the message
        /// </summary>
        /// <param name="startTime">The number of seconds the counter will take to get to zero</param>
        /// <param name="counterRate">The time inbetween the time being displayed</param>
        /// <param name="messageDisplayTime">The amount of time that the message that displays after the counter is done will be visable for</param>
        public IEnumerator DoCountdownTimer(float startTime, float counterRate, float messageDisplayTime) {
            float time = startTime;
            playerUI.CountdownTimer.gameObject.SetActive(true);
            while (time > 0) {
                //string timeAsString = time.ToString().Substring(0, 3);
                playerUI.CountdownTimer.SetText(time.ToString("F1"));
                time -= counterRate;
                yield return new WaitForSeconds(counterRate);
            }
            //--Display message for a few seconds once countdown has reached zero
            playerUI.CountdownTimer.SetText("GO");
            yield return new WaitForSeconds(messageDisplayTime);
            playerUI.CountdownTimer.gameObject.SetActive(false);

        }
        //=== SWITCH BETWEEN RUNNER AND CHASER UI ===

        public void SwitchToRunnerUI() {
            runnerUI.RunnerUIObject.SetActive(true);
            chaserUI.ChaserUIObject.SetActive(false);

        }
        public void SwitchToChaserUI() {
            chaserUI.ChaserUIObject.SetActive(true);
            runnerUI.RunnerUIObject.SetActive(false);
        }
        //=== RUNNER CONTROLS ===

        public void SetPlacedCheckpointTracker(float currentDst, float dstBetweenCheckpoint) {
            runnerUI.PlaceCheckpointTracker.maxValue = dstBetweenCheckpoint;
            runnerUI.PlaceCheckpointTracker.value = currentDst;
        }

        public void SetCheckpointsAheadText(int cpAhead) {
            runnerUI.CheckpointsAhead.SetText(cpAhead.ToString());
        }

        /// <summary>
        /// Sets the distance tracker on the runner UI
        /// </summary>
        public void SetDistanceTrackerUI(float distanceTravelled, float targetDistance) {
            runnerUI.DistanceTrackerSlider.maxValue = targetDistance;
            runnerUI.DistanceTrackerSlider.value = distanceTravelled;
            runnerUI.DistanceTrackerText.SetText(distanceTravelled.ToString("F0") + "/" + targetDistance.ToString("F0"));
        }
        /// <summary>
        /// Sets the distance tracker on the chaser UI
        /// </summary>
        public void SetDistanceTrackerChaserUI(float distanceTravelled, float targetDistance) {
            chaserUI.RunnerDistanceTrackerSlider.maxValue = targetDistance;
            chaserUI.RunnerDistanceTrackerSlider.value = distanceTravelled;
            chaserUI.RunnerDistanceTrackerText.SetText(distanceTravelled.ToString("F0") + "/" + targetDistance.ToString("F0"));
        }
        //=== CHASER CONTROLS ===

        

       
    }
}
