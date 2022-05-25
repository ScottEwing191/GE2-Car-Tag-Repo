using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Abilities;
using CarTag.UI;
using CarTag.PlayerSpace;
using CarTag.ScoreSystem;
using CarTag.Rounds;



namespace CarTag.Abilities {
    public class AbilityManager : MonoBehaviour {
        public enum AbilityType { SLOWMO, BOX, ROCKET }
        //--Auto-Implemented Properties
        public List<PlayerAbilityController> PlayerAbilityControllers { get; set; }
        public PlayerManager PlayerManager { get; private set; }
        public UIManager UIManager { get; set; }
        public RoundManager RoundManager { get; set; }
        public ScoreManager ScoreManager { get; set; }




        //=== PUBLIC METHODS ===
        public void InitialSetup() {
            PlayerAbilityControllers = new List<PlayerAbilityController>();
            UIManager = GameManager.Instance.UIManager;
            PlayerManager = GameManager.Instance.PlayerManager;
            RoundManager = GameManager.Instance.RoundManager;
            ScoreManager = GameManager.Instance.ScoreManager;
            foreach (Player player in PlayerManager.Players) {
                PlayerAbilityControllers.Add(player.GetComponentInChildren<PlayerAbilityController>());
            }
            foreach (PlayerAbilityController p in PlayerAbilityControllers) {
                p.InitialSetup();
            }
        }

        private void OnEnable() {
            GameEvents.onRoleSwap += ResetAbilities;
        }

        private void OnDisable() {
            GameEvents.onRoleSwap -= ResetAbilities;
        }

        public void ResetAbilities(Player newRunner, Player unusedNewChaser = null) {
            newRunner.PlayerAbilityController.ResetAbilities(true);
            foreach (var c in PlayerAbilityControllers) {
                if (c == newRunner.PlayerAbilityController) {
                    continue;
                }
                c.ResetAbilities(false);
            }
        }

        

        public bool IsControllerAttachedToRunner(PlayerAbilityController controller) {
            if (controller == GetRunnerAbilityController()) {
                return true;
            }
            return false;
        }
        //=== PRIVATE METHODS ===
        public PlayerAbilityController GetRunnerAbilityController() {
            return PlayerAbilityControllers[PlayerManager.CurrentRunner.PlayerListIndex];
        }
    }
}
