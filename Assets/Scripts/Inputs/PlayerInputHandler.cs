using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CarTag.PlayerSpace;

namespace CarTag.Input
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] Player player;
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
    }
}
