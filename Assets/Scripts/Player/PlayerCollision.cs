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
            if (player.PlayerRoll != PlayerRoleEnum.Chaser) {
                print(transform.parent.name + " : This car is not a Chaser");
                return; }    //this car must be a chaser to continue

            if (!collision.transform.CompareTag("Player")) {
                print(transform.parent.name + " : Other Object is not a Player");
                return; }      // other object must be a car to continue

            if (!IsCollidingWithRunner(collision)) {
                print(transform.parent.name + " : Colliding with other Chaser");
                return; }              // Must be colliding with the runner and not another chaser to continue

            //if (GameManager.Instance.CheckpointManager.Checkpoints.Count != 0) { return; }  // Has the other car been trhough the required checkpoints
            if (GameManager.Instance.CheckpointManager.CheckpointQueues[player.PlayerListIndex].Count != 0) {
                print(transform.parent.name+ " : Checkpoint Missed");
                return; }  // Has this car been through the required checkpoints


            //Initiate roll Swap
            print(transform.parent.name + "Swap Rolls");
            print("This: " + transform.parent.name + " Collision: " + collision.gameObject.name);
        }

        private bool IsCollidingWithRunner(Collision collision) {
            foreach (var p in player.playerManager.Players) {
                if (p.gameObject == collision.gameObject.transform.parent.gameObject) {         // if this Player is the one we are colliding with
                    if (p.PlayerRoll == PlayerRoleEnum.Runner) {                                // .. and this player is the runner
                        //print("Correct Car");
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
