using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.PlayerSpace;

namespace CarTag.UI {
    public class UIManager : MonoBehaviour {
        //--Serialized Fields
        [Tooltip("The time between counter updates as it is counting down")]
        [SerializeField] private float counterRate = 0.1f;
        
        //--Private 
        private List<PlayerUIController> playerUIControllers = new List<PlayerUIController>();

        //--Auto-Implemented Properties
        public PlayerManager PlayerManager { get; private set; }

        //--Properties
        public List<PlayerUIController> PlayerUIControllers { get { return playerUIControllers; } }

        //--Methods
        public void InitalSetup() {
            PlayerManager = GameManager.Instance.PlayerManager;
            foreach (Player player in PlayerManager.Players) {
                playerUIControllers.Add(player.GetComponentInChildren<PlayerUIController>());
            }
        }
        public void StartRunnerCountdown(float startTime) {
            //StartCoroutine(StartCountdownTimer(startTime, PlayerManager.CurrentRunner.PlayerListIndex, true));
            PlayerUIController runnerUI = playerUIControllers[PlayerManager.CurrentRunner.PlayerListIndex];
            StartCoroutine(runnerUI.DoCountdownTimer(startTime, counterRate, 1));
        }
        public void StartChaserCountdown(float startTime) {
            //StartCoroutine(StartCountdownTimer(startTime, PlayerManager.CurrentRunner.PlayerListIndex));
            for (int i = 0; i < playerUIControllers.Count; i++) {
                if (i == PlayerManager.CurrentRunner.PlayerListIndex)
                    continue;
                StartCoroutine(playerUIControllers[PlayerManager.CurrentRunner.PlayerListIndex].DoCountdownTimer(startTime, counterRate, 2));
            }
        }

        /*public IEnumerator StartCountdownTimer(float startTime, int runnerIndex, bool isRunnerTimer = false) {
            float time = startTime;
            while (time > 0) {
                //--If setting the runner timer
                if (isRunnerTimer) {
                    playerUIControllers[runnerIndex].SetCountdownTime(time.ToString());
                }
                //--If setting the chaser timers
                else  {
                    for (int i = 0; i < playerUIControllers.Count; i++) {
                        if (i == runnerIndex)
                            continue;
                        playerUIControllers[i].SetCountdownTime(time.ToString());
                    } 
                }
                time -= counterRate;
                yield return new WaitForSeconds(counterRate);
            }

            //--Display message once countdown has reached zero
            //--If setting the runner message
            if (isRunnerTimer) {
                playerUIControllers[runnerIndex].SetCountdownTime("START");
            }
            //--If setting the chaser messages
            else {
                for (int i = 0; i < playerUIControllers.Count; i++) {
                    if (i == runnerIndex)
                        continue;
                    playerUIControllers[i].SetCountdownTime("START");
                }
            }

            yield return 

            
        }*/
    }
}
