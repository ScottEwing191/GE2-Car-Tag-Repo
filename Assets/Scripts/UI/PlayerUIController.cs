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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime">The number of seconds the counter will take to get to zero</param>
        /// <param name="counterRate">The time inbetween the time being displayed</param>
        /// <param name="messageDisplayTime">The amount of time that the message that displays after the counter is done will be visable for</param>
        /// <returns></returns>
        public IEnumerator DoCountdownTimer(float startTime, float counterRate, float messageDisplayTime) {
            float time = startTime;
            countdownTimer.gameObject.SetActive(true);
            while (time > 0) {
                //string timeAsString = time.ToString().Substring(0, 3);
                countdownTimer.SetText(time.ToString("F1"));
                time -= counterRate;
                yield return new WaitForSeconds(counterRate);
            }
            //--Display message for a few seconds once countdown has reached zero
            countdownTimer.SetText("GO");
            yield return new WaitForSeconds(messageDisplayTime);
            countdownTimer.gameObject.SetActive(false);

        }
    }
}
