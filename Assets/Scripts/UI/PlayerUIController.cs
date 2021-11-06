using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CarTag.UI {
    public class PlayerUIController : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI countdownTimer;

        //--Properties
        public TextMeshProUGUI CountdownTimer {
            get { return countdownTimer; }
            set { countdownTimer = value; }
        }

        //--Methods
        public IEnumerator DoCountdownTimer(float startTime, float counterRate, float messageDisplayTime) {
            float time = startTime;
            countdownTimer.gameObject.SetActive(true);
            while (time > 0) {
                countdownTimer.SetText(time.ToString());
                time -= counterRate;
                yield return new WaitForSeconds(counterRate);
            }
            //--Display message for a few seconds once countdown has reached zero
            countdownTimer.SetText("START");
            yield return new WaitForSeconds(messageDisplayTime);
            countdownTimer.gameObject.SetActive(false);

        }
    }
}
