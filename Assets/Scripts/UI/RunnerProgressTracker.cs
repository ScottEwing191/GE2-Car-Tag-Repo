using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CarTag.UI
{
    public class RunnerProgressTracker{
        private Slider _runnerSlider;
        private Slider _chaserSlider;
        private TextMeshProUGUI _runnerText;
        private TextMeshProUGUI _chaserText;
        

        public RunnerProgressTracker(Slider runnerSlider, Slider chaserSlider, TextMeshProUGUI runnerText, TextMeshProUGUI chaserText) {
            _runnerSlider = runnerSlider;
            _chaserSlider = chaserSlider;
            _runnerText = runnerText;
            _chaserText = chaserText;
        }


        public void SetDistanceTrackerUI(Slider slider, float distanceTravelled, float targetDistance) {
            slider.maxValue = targetDistance;
            slider.value = distanceTravelled;
            runnerUI.DistanceTrackerText.SetText(distanceTravelled.ToString("F0") + "/" + targetDistance.ToString("F0"));
        }
    }
}
