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
        [SerializeField] Player player;
        [SerializeField] PlayerAbilityController abilityController; 
        int test = 0;

        private void Update() {
            test++;
        }
        public void OnRespawn(InputAction.CallbackContext context) {
            //print("Respawn");
            if (context.started) {
                //print("Respawn Started");
                //--Start UI Respawn Display
            }
            if (context.performed) {
                //print("Respawn Performed");
                player.PlayerRespawn.RespawnAtCheckpoint();

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
                print("Next Ability");
                abilityController.NextAbility();
            }
        }
        public void OnPreviousAbility(InputAction.CallbackContext context) {
            if (context.started) {
                print("Previous Ability");
                abilityController.PreviousAbility();
            }
        }
    }
}
