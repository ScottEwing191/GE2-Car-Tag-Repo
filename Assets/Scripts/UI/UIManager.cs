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
        /// <summary>
        /// Using the list of player from the Player Manager find all the PlayerUIController Components and add them to the  UI Manager's list
        /// This will make sure the players are in the same order win both lists
        /// </summary>
        public void InitalSetup() {
            PlayerManager = GameManager.Instance.PlayerManager;
            foreach (Player player in PlayerManager.Players) {
                playerUIControllers.Add(player.GetComponentInChildren<PlayerUIController>());
            }
        }

        /// <summary>
        /// Starts the countdown timer on the runner
        /// </summary>
        public void StartRunnerCountdown(float startTime) {
            PlayerUIController runnerUI = playerUIControllers[PlayerManager.CurrentRunner.PlayerListIndex];
            StartCoroutine(runnerUI.DoCountdownTimer(startTime, counterRate, 1));
        }

        /// <summary>
        /// Starts the countdown timer on the Chasers. Skips the runner
        /// </summary>
        public void StartChaserCountdown(float startTime) {
            for (int i = 0; i < playerUIControllers.Count; i++) {
                if (i != PlayerManager.CurrentRunner.PlayerListIndex) {
                    StartCoroutine(playerUIControllers[i].DoCountdownTimer(startTime, counterRate, 2));
                }
            }
        }

       
    }
}
