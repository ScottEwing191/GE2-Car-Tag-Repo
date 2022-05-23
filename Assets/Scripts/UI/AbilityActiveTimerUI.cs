using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace CarTag.UI {
    public class AbilityActiveTimerUI : MonoBehaviour {
        private Coroutine _startTimerUIRoutine;

        public Slider TimerSlider { get; set; }

        public TextMeshProUGUI AbilityActiveText { get; set; }

        private void Awake() {
            TimerSlider = GetComponentInChildren<Slider>();
            AbilityActiveText = GetComponentInChildren<TextMeshProUGUI>();
            TimerSlider.gameObject.SetActive(false);
            AbilityActiveText.gameObject.SetActive(false);
        }

        public void StartTimerUI(string text, float duration) {
            if (_startTimerUIRoutine == null) {
                _startTimerUIRoutine = StartCoroutine(StartTimerUIRoutine(text, duration));
            }
        }

        public void StopTimerUI() {
            if (_startTimerUIRoutine != null) {
                StopCoroutine(_startTimerUIRoutine);
                TimerSlider.gameObject.SetActive(false);
                AbilityActiveText.gameObject.SetActive(false);
                _startTimerUIRoutine = null;
            }
        }

        public IEnumerator StartTimerUIRoutine(string text, float duration) {
            TimerSlider.gameObject.SetActive(true);
            AbilityActiveText.gameObject.SetActive(true);
            AbilityActiveText.SetText(text);
            TimerSlider.maxValue = duration;
            TimerSlider.value = duration;

            float time = duration;
            while (time > 0) {
                TimerSlider.value = time;
                time -= Time.deltaTime;
                yield return null;
            }
            TimerSlider.gameObject.SetActive(false);
            AbilityActiveText.gameObject.SetActive(false);
            _startTimerUIRoutine = null;
        }
    }
}
