using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Checkpoints {
    public class CheckpointReached {
        private CheckpointManager checkpointManager;
        public CheckpointReached(CheckpointManager checkpointManager) {
            this.checkpointManager = checkpointManager;
        }

        public void DoCheckpointReached(Checkpoint triggeredheckpoint, PlayerSpace.Player player) {
            if (!CheckIfCorrectPlayer(player)) { return; }                // Check if the correct player collided

            if (CheckIfCorrectCheckpoint(triggeredheckpoint, player.PlayerListIndex)) {     // Check if the correct checkpoint was collided with
                CheckpointSucessfullyReached(triggeredheckpoint, player.PlayerListIndex);                     // Successfull Checkpoint Reached
            }
            else {
                WrongCheckpoint();                                  // Wrong checkpoint was reached
            }
        }

        /// <summary>
        /// Checks if it was the correct player who collided with the checkpoint i.e the chaser not the runner
        /// </summary>
        private bool CheckIfCorrectPlayer(PlayerSpace.Player player) {
            // I HATE THIS - if the player is a child of of an a "Player" and that player has the role Runner then return true  
            if (player != null) {
                if (player.PlayerRoll == PlayerSpace.PlayerRoleEnum.Chaser) {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Takes in the checkpoint collided with and the index of the queue to check, and determines if the checkpoint is the next item in the queue or not
        /// </summary>
        private bool CheckIfCorrectCheckpoint(Checkpoint cp, int index) {
            if (checkpointManager.CheckpointQueues[index].Peek() == cp) {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Removes the checkpoint from the Queue an letts the checkpoint manager know that checkpoint was sucessfully reached
        /// </summary>
        private void CheckpointSucessfullyReached(Checkpoint cp, int index) {
            checkpointManager.CheckpointQueues[index].Dequeue();        // remove checkpoint from the queue
            cp.CheckpointReached(index);                         // call destroy checkpoint method. May want to have animation or something later
            checkpointManager.CheckpointVisibility.UpdateVisibleCheckpoints(index);
        }

        /// <summary>
        /// Handles what happens if the wrong checkpoint is reached
        /// </summary>
        private void WrongCheckpoint() {
        }
    }
}
