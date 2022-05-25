using System;
using System.Collections;
using System.Collections.Generic;
using CarTag.UI.ProgressTracker;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

namespace CarTag.UI {
    public class PlayerUIController : MonoBehaviour {
        //--Serialized Fields
        [SerializeField] PlayerUIElements playerUI;
        [SerializeField] RunnerUIElements runnerUI;
        [SerializeField] ChaserUIElements chaserUI;
        [SerializeField] private RunnerProgressTrackerUIElements runnerProgressTrackerUIElements;
        private Coroutine _doCountdownTimerRoutine;
        [field: SerializeField] public HoldButtonUI CheckpointResetButtonUI { get; set; }
        [field: SerializeField] public HoldButtonUI ForfeitButtonUI { get; set; }


        //--Private           
        public Player thisPlayer { get; set; }  // used to gain acess to other controller script attached to the same player as this one
        
        //--Auto-Implemented Properties
        public RunnerProgressTracker RunnerProgressTracker { get; private set; }
        public ChaserCheckpointTracker ChaserCheckpointTracker { get; set; }
        public AbilityUI AbilityUI { get; set; }
        public ScreenFadeUI ScreenFadeUI { get; set; }
        public CheckpointGuideUI CheckpointGuideUI { get; private set; }

        private void Awake() {
            ChaserCheckpointTracker = new ChaserCheckpointTracker(chaserUI.CheckpointTracker);
            AbilityUI = GetComponentInChildren<AbilityUI>();
            ScreenFadeUI = GetComponentInChildren<ScreenFadeUI>();
            thisPlayer = GetComponentInParent<Player>();
            CheckpointGuideUI = GetComponentInChildren<CheckpointGuideUI>();
            //CheckpointResetButtonUI = GetComponentInChildren<HoldButtonUI>();
            RunnerProgressTracker = new RunnerProgressTracker(runnerProgressTrackerUIElements);
        }

        public void InitialSetup() {
            AbilityUI.InitialSetup(thisPlayer.PlayerAbilityController.CurrentAbility.UsesLeft, thisPlayer.IsThisPlayerCurrentRunner());
        } 
        //--Methods
        public void DoCountdownTimer(float startTime, float counterRate, float messageDisplayTime) {
            if (_doCountdownTimerRoutine != null) {
                StopCoroutine(_doCountdownTimerRoutine);
            }
            _doCountdownTimerRoutine = StartCoroutine(DoCountdownTimerRoutine(startTime, counterRate, messageDisplayTime));
        }


        /// <summary>
        /// Start the countdown timer at the given time, When the timer reaches 0 display a message for the given time then hide the message
        /// </summary>
        /// <param name="startTime">The number of seconds the counter will take to get to zero</param>
        /// <param name="counterRate">The time inbetween the time being displayed</param>
        /// <param name="messageDisplayTime">The amount of time that the message that displays after the counter is done will be visable for</param>
        private IEnumerator DoCountdownTimerRoutine(float startTime, float counterRate, float messageDisplayTime) {
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
            _doCountdownTimerRoutine = null;
        }
        //=== ENABLE/DISABLE RUNNER/ CHASER/ PLAYER UI===

        public void SwitchToRunnerUI() {
            runnerUI.RunnerUIObject.SetActive(true);
            chaserUI.ChaserUIObject.SetActive(false);

        }
        public void SwitchToChaserUI() {
            chaserUI.ChaserUIObject.SetActive(true);
            runnerUI.RunnerUIObject.SetActive(false);
        }
        public void EnablePlayerUI() {
            playerUI.PlayerUIObject.SetActive(true);
        }
        //-- Sets the Runner/ Chaser/ Player/ UI gamobjects to Inactive
        public void DisableUI() {
            chaserUI.ChaserUIObject.SetActive(false);
            runnerUI.RunnerUIObject.SetActive(false);
            playerUI.PlayerUIObject.SetActive(false);
        }

        public void ResetUI(bool isNewRunner) {
            SetCheckpointsAheadText(0);                       // reset runner's checkpoints ahead tracker
            SetPlacedCheckpointTracker(0, 0);                 // reset runner's checkpoint placed slider
                
            ChaserCheckpointTracker.ResetCpTracker();         // reset chaser's checkpoint tracker
            // Ability UI Is Reset from the PlayerAbilityController
            EnablePlayerUI();                                 // make sure Player UI is on
            if (isNewRunner) 
                SwitchToRunnerUI();
            else 
                SwitchToChaserUI();
        }

        //--Runner Checkpoint Placed UI 
        public void SetPlacedCheckpointTracker(float currentDst, float dstBetweenCheckpoint) {
            runnerUI.PlaceCheckpointTracker.maxValue = dstBetweenCheckpoint;
            runnerUI.PlaceCheckpointTracker.value = currentDst;
        }

        public void SetCheckpointsAheadText(int cpAhead) {
            runnerUI.CheckpointsAhead.SetText(cpAhead.ToString());
        }

        /// <summary>
        /// Sets the distance tracker on the Player's UI
        /// </summary>
        public void SetDistanceTrackerUI(float distanceTravelled, float targetDistance) {
            RunnerProgressTracker.SetDistanceTrackerUI(distanceTravelled, targetDistance, thisPlayer.IsThisPlayerCurrentRunner());
        }
        
    }
}
