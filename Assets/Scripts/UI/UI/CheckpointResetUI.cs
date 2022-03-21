using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//==============================================================================================
//--Checkpoint Reset UI show a visual representation of the players input while holding the reset button.
//--The Reset UI will also be displayed if the player gets further away from the checkpoint to prompt
//--the player to use the Reset feature.
//==============================================================================================


namespace CarTag.UI {
    public class CheckpointResetUI : MonoBehaviour {
        [SerializeField] private Image filledImage;
        private bool _isButtonHeld;
        private float _timer = 0.0f;
        private float _holdTime;

        public void StartResetHold(float holdTime) {
            filledImage.gameObject.SetActive(true);
            _timer = 0;
            _holdTime = holdTime;
            _isButtonHeld = true;
            filledImage.fillAmount = 0;
        }
        public void StopResetHold() {
            _isButtonHeld = false;
            filledImage.gameObject.SetActive(false);
        }

        public void ShowResetButtonUI() {
            filledImage.gameObject.SetActive(true);
            filledImage.fillAmount = 0;
        }
        public void HideResetButtonUI() { 
            filledImage.gameObject.SetActive(false);
        }

        private void Update() {
            if (!_isButtonHeld) { return; }
            if (_timer > _holdTime) { return; }
            _timer += Time.deltaTime;
            filledImage.fillAmount = _timer / _holdTime;
        }
    }
}
