using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CarTag.PlayerSpace;
using CarTag.Abilities;

namespace CarTag.Input {
    public class PlayerInputHandler : MonoBehaviour {
        Player thisPlayer;
        [SerializeField] PlayerAbilityController abilityController;

        public InputManager InputManager { get; private set; }

        private void Start() {
            InputManager = GetComponent<InputManager>();
            thisPlayer = GetComponent<Player>();
        }

        public void OnRespawn(InputAction.CallbackContext context) {
            if (Time.timeScale == 0) { return; }
            if (context.performed) {
                thisPlayer.PlayerRespawn.RespawnAtCheckpoint();
            }
        }

        public void OnUseAbility(InputAction.CallbackContext context) {
            if (Time.timeScale == 0) { return; }
            if (context.started) {
                abilityController.OnAbilityInputStarted();
            }
            if (context.canceled) {
                abilityController.OnAbilityInputCancelled();
            }
        }

        public void OnNextAbility(InputAction.CallbackContext context) {
            if (Time.timeScale == 0) { return; }
            if (context.started) {
                abilityController.NextAbility();
            }
        }
        public void OnPreviousAbility(InputAction.CallbackContext context) {
            if (Time.timeScale == 0) { return; }
            if (context.started) {
                abilityController.PreviousAbility();
            }
        }

        public void OnPauseMenu(InputAction.CallbackContext context) {
            if (context.started) {
                InputManager.UIManager.LevelUI.DoPauseMenu();
            }
        }
    }
}
