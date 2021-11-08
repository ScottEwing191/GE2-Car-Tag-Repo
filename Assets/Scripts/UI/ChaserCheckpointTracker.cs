using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CarTag.UI {
    public class ChaserCheckpointTracker {
        //--Serialized Field
        [Tooltip("The min number of checkpoint markers which will be used to display the chaser progress through checkpoints")]
        [SerializeField] private int minChaserCpProgressCount = 5;
        [Tooltip("The max number of checkpoint markeprs which will be used to display the chaser progress through checkpoints")]
        [SerializeField] private int maxChaserCpProgressCount = 19;

        //--Private 
        private float cpIconWidth;
        private float sliderWidth;
        private int activeIconCount;     // the number of icon displayed on the UI. Found by clamping cpMadeBeforeChaserStart between...
                                         //... minChaserCpProgressCount and maxChaserCpProgressCount
        private List<Image> activeCpIcons = new List<Image>();
        private ChaserCheckpointTrackerUIElements cpTrackerElements;

        //--Auto-Implemented Properties

        //=== Constructor ===
        public ChaserCheckpointTracker(ChaserCheckpointTrackerUIElements cpTrackerElements) {
            this.cpTrackerElements = cpTrackerElements;
            this.sliderWidth = cpTrackerElements.ChaserCpTrackerSlider.GetComponent<RectTransform>().rect.width;
            this.cpIconWidth = cpTrackerElements.DefualtCheckpointIcons[0].GetComponent<RectTransform>().rect.width;
        }

        //=== Public Methods ===
        public void SetupCpTracker(int cpMadeBeforeChaserStart) {
            float interiorIconsCountFloat = (float)cpMadeBeforeChaserStart + ((float)cpMadeBeforeChaserStart / 2);

            int interiorIconsCount = (int)System.Math.Round((double)interiorIconsCountFloat, System.MidpointRounding.AwayFromZero);

            activeIconCount = interiorIconsCount + 2;       // +1 for start icon and +1 for the end Icon
            activeIconCount = Mathf.Clamp(activeIconCount, minChaserCpProgressCount, maxChaserCpProgressCount);
            activeCpIcons.Clear();
            DisableCheckpointIcons();
            activeCpIcons.Add(cpTrackerElements.StartCheckpointIcon);
            for (int i = 0; i < activeIconCount - 2; i++) {
                activeCpIcons.Add(cpTrackerElements.DefualtCheckpointIcons[i]);
            }
            activeCpIcons.Add(cpTrackerElements.EndCheckpointIcon);
            EnableActiveIcons(activeCpIcons);
            SetSpacing(activeIconCount);
            SetCpTrackerSlider(cpMadeBeforeChaserStart, activeIconCount);
            SetNumberIcon(cpMadeBeforeChaserStart);
        }

        /// <summary>
        /// Sets the CP tracker to the minimum number of icons with zero checkpoints Left 
        /// </summary>
        public void ResetCpTracker() {
            SetupCpTracker(0);
        }

        public void UpdateTracker(int checkpointsAhead) {
            SetCpTrackerSlider(checkpointsAhead);
            SetNumberIcon(checkpointsAhead);
        }
        //=== Private Methods ===
        private void SetCpTrackerSlider(int checkpointsLeft, int totalIconsCount) {
            cpTrackerElements.ChaserCpTrackerSlider.maxValue = totalIconsCount - 1;
            cpTrackerElements.ChaserCpTrackerSlider.value = cpTrackerElements.ChaserCpTrackerSlider.maxValue - checkpointsLeft;
        }
        private void SetCpTrackerSlider(int checkpointsLeft) {
            cpTrackerElements.ChaserCpTrackerSlider.value = cpTrackerElements.ChaserCpTrackerSlider.maxValue - checkpointsLeft;
        }
        private void SetNumberIcon(int checkpointsLeft) {
            int targetIndex;
            if (checkpointsLeft == 0) {
                targetIndex = activeCpIcons.Count - 1;
            }
            else if (checkpointsLeft > (activeIconCount - 2)) {
                targetIndex = 0;
            }
            else {
                targetIndex = (activeCpIcons.Count - 1) - checkpointsLeft;
            }
            RectTransform rt = cpTrackerElements.NumberCheckpointIcon.rectTransform;
            rt.anchoredPosition = new Vector3((cpIconWidth + cpTrackerElements.LayoutGroup.spacing) * targetIndex, rt.anchoredPosition.y);
            cpTrackerElements.CheckpointsLeftText.SetText(checkpointsLeft.ToString());
        }

        private void DisableCheckpointIcons() {
            cpTrackerElements.StartCheckpointIcon.gameObject.SetActive(false);
            cpTrackerElements.EndCheckpointIcon.gameObject.SetActive(false);
            cpTrackerElements.NumberCheckpointIcon.gameObject.SetActive(false);
            for (int i = 0; i < cpTrackerElements.DefualtCheckpointIcons.Count; i++) {
                cpTrackerElements.DefualtCheckpointIcons[i].gameObject.SetActive(false);

            }
        }

        private void EnableActiveIcons(List<Image> activeIcons) {
            for (int i = 0; i < activeIcons.Count; i++) {
                activeIcons[i].gameObject.SetActive(true);
            }
            cpTrackerElements.NumberCheckpointIcon.gameObject.SetActive(true);
        }

        private void SetSpacing(float activeIconCount) {
            float iconsWidth = (activeIconCount -1) * cpIconWidth;
            float widthMinusIcons = sliderWidth - iconsWidth;
            cpTrackerElements.LayoutGroup.spacing = (widthMinusIcons / (activeIconCount - 1));
        }
    }
}
