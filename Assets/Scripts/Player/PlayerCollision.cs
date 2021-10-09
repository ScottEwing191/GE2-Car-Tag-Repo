using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Player {
    public class PlayerCollision : MonoBehaviour {
        Player player;
        private void Awake() {
            player = GetComponentInParent<Player>();
        }

        private void OnCollisionEnter(Collision collision) {
            print("Collision");
            if (player.PlayerRoll != PlayerRoleEnum.Chaser) { return; }    //is this car a Chaser

            if (!collision.transform.CompareTag("Player")) { return; }      // is the other object a car

            if (!IsCollidingWithRunner(collision)) { return; }              // are we colliding with the Runner 

            if (GameManager.Instance.CheckpointManager.Checkpoints.Count != 0) { return; }  // Has the other car been trhough the required checkpoints


            //Initiate roll Swap
            print("Swap Rolls");
        }

        private bool IsCollidingWithRunner(Collision collision) {
            foreach (var p in player.playerManager.Players) {
                if (p.gameObject == collision.gameObject.transform.parent.gameObject) {         // if this Player is the one we are colliding with
                    if (p.PlayerRoll == PlayerRoleEnum.Runner) {                                // .. and this player is the runner
                        print("Correct Car");
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
