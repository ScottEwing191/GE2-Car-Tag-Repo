using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CarTag.PlayerSpace;
using CarTag.Abilities;

namespace CarTag.Input
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] Player thisPlayer;
        [SerializeField] PlayerAbilityController abilityController;

        public InputManager InputManager { get; private set; }

        private void Start() {
            InputManager = GetComponent<InputManager>();
        }

        public void OnRespawn(InputAction.CallbackContext context) {
            //print("Respawn");
            if (context.started) {
                //print("Respawn Started");
                //--Start UI Respawn Display
            }
            if (context.performed) {
                //print("Respawn Performed");
                thisPlayer.PlayerRespawn.RespawnAtCheckpoint();

            }
            if (context.canceled) {
                //print("Respawn Cancelled");
                //--Stop UI Respawn Display
            }
            
        }

        public void OnUseAbility(InputAction.CallbackContext context) {
            if (context.started) {
                abilityController.OnAbilityInputStarted();
            }
            if (context.canceled) {
                abilityController.OnAbilityInputCancelled();
            }
        }

        public void OnNextAbility(InputAction.CallbackContext context) {
            if (context.started) {
                abilityController.NextAbility();
            }
        }
        public void OnPreviousAbility(InputAction.CallbackContext context) {
            if (context.started) {
                abilityController.PreviousAbility();
            }
        }

        public void OnPauseMenu(InputAction.CallbackContext context) {
            if (context.started) {
                print("Pause Menu");
                InputManager.UIManager.LevelUI.DoPauseMenu();
            }
        }
    }
}
