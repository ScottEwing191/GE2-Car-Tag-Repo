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
                //abilityController.OnAbilityInput(InputState.STARTED);
                abilityController.OnAbilityInputStarted();

            }
            /*if (context.performed) {
                abilityController.OnAbilityInput(InputState.PERFORMED);
            }*/
            if (context.canceled) {
                //abilityController.OnAbilityInput(InputState.CANCELLED);
                abilityController.OnAbilityInputCancelled();

            }
        }
    }
}
