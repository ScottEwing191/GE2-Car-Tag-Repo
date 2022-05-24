using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CarTag.UI.ProgressTracker
{
    public class RunnerProgressTracker{
        /*private Slider _runnerSlider;
        private Slider _chaserSlider;
        private TextMeshProUGUI _runnerText;
        private TextMeshProUGUI _chaserText;*/
        private RunnerProgressTrackerUIElements _elements;

        /*public RunnerProgressTracker(Slider runnerSlider, Slider chaserSlider, TextMeshProUGUI runnerText, TextMeshProUGUI chaserText) {
            _runnerSlider = runnerSlider;
            _chaserSlider = chaserSlider;
            _runnerText = runnerText;
            _chaserText = chaserText;
        }*/

        public RunnerProgressTracker(RunnerProgressTrackerUIElements elements) {
            _elements = elements;
        }

        public void SetDistanceTrackerUI(float distanceTravelled, float targetDistance, bool isRunner) {
            if (isRunner) {
                SetUI(_elements.ProgressTrackerSliderForRunner, _elements.ProgressTrackerTextForRunner, distanceTravelled, targetDistance);
                return;
            }
            SetUI(_elements.ProgressTrackerSliderForChaser, _elements.ProgressTrackerTextForChaser, distanceTravelled, targetDistance);
        }

        private void SetUI(Slider slider, TextMeshProUGUI text, float distanceTravelled, float targetDistance) {
            slider.maxValue = targetDistance;
            slider.value = distanceTravelled;
            text.SetText(distanceTravelled.ToString("F0") + "/" + targetDistance.ToString("F0"));
        }
    }
}
