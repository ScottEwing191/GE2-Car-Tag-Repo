using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Checkpoints {
    public class CheckpointReached {
        private CheckpointManager checkpointManager;
        public CheckpointReached(CheckpointManager checkpointManager) {
            this.checkpointManager = checkpointManager;
        }

        public void DoCheckpointReached(Checkpoint triggeredheckpoint, Collider playerTrigger) {
            if (!CheckIfCorrectPlayer(playerTrigger)) { return; }                // Check if the correct player collided

            if (CheckIfCorrectCheckpoint(triggeredheckpoint)) {     // Check if the correct checkpoint was collided with
                CheckpointSucessfullyReached(triggeredheckpoint);                     // Successfull Checkpoint Reached
            }
            else {
                WrongCheckpoint();                                  // Wrong checkpoint was reached
            }
        }

        /// <summary>
        /// Checks if it was the correct player who collided with the checkpoint i.e the chaser to the runner
        /// </summary>
        private bool CheckIfCorrectPlayer(Collider playerTrigger) {
            // I HATE THIS - if the player is a child of of an a "Player" and that player has the role Runner then return true  
            Player.Player tempPlayer = playerTrigger.GetComponentInParent<CarTag.Player.Player>();
            if (tempPlayer != null) {
                if (tempPlayer.PlayerRoll == Player.PlayerRoleEnum.Chaser) {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if the checkpoint collided with is the first in the queue
        /// </summary>
        private bool CheckIfCorrectCheckpoint(Checkpoint cp) {
            if (checkpointManager.Checkpoints.Peek() == cp) {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Removes the checkpoint from the Queue an letts the checkpoint manager know that checkpoint was sucessfully reached
        /// </summary>
        private void CheckpointSucessfullyReached(Checkpoint cp) {
            checkpointManager.Checkpoints.Dequeue();        // remove checkpoint from the queue
            cp.DestroyCheckpoint();                         // call destroy checkpoint method. May want to have animation or something later
            checkpointManager.CheckpointVisibility.UpdateVisibleCheckpoints();
        }

        /// <summary>
        /// Handles what happens if the wrong checkpoint is reached
        /// </summary>
        private void WrongCheckpoint() {
            Debug.Log("Wrong Checkpoint");
        }
    }
}
