using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CarTag.Checkpoints
{
    public class CheckpointReached
    {
        public static Action OnCheckpointReached = delegate { };
        private CheckpointManager checkpointManager;

        public CheckpointReached(CheckpointManager checkpointManager)
        {
            this.checkpointManager = checkpointManager;
        }

        public void DoCheckpointReached(Checkpoint triggeredheckpoint, Player player)
        {
            if (!CheckIfCorrectPlayer(player)) { return; }                                  // Check if the correct player collided

            if (CheckIfCorrectCheckpoint(triggeredheckpoint, player.PlayerListIndex))
            {     // Check if the correct checkpoint was collided with
                CheckpointSucessfullyReached(player, triggeredheckpoint, player.PlayerListIndex);   // Successfull Checkpoint Reached
            }
            else
            {
                WrongCheckpoint();                                                          // Wrong checkpoint was reached
            }
        }

        /// <summary>
        /// Checks if it was the correct player who collided with the checkpoint i.e the chaser not the runner
        /// </summary>
        private bool CheckIfCorrectPlayer(Player player)
        {
            // I HATE THIS - if the player is a child of of an a "Player" and that player has the role Runner then return true  
            if (player != null)
            {
                if (player.PlayerRoll == PlayerRoleEnum.Chaser)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Takes in the checkpoint collided with and the index of the queue to check, and determines if the checkpoint is the next item in the queue or not
        /// </summary>
        private bool CheckIfCorrectCheckpoint(Checkpoint cp, int index)
        {
            if (checkpointManager.CheckpointQueues[index].Peek() == cp)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Removes the checkpoint from the Queue an lets the checkpoint manager know that checkpoint was sucessfully reached
        /// </summary>
        /// <param name="cp">The checkpoint which has just been reached</param>
        /// <param name="queueListIndex">The index of the player who collided with the checkpoint</param>
        private void CheckpointSucessfullyReached(Player thisPlayer, Checkpoint cp, int queueListIndex)
        {
            thisPlayer.PlayerRespawn.SetRespawnLocation(cp.respawnPosition, cp.respawnRotation);
            checkpointManager.CheckpointQueues[queueListIndex].Dequeue();        // remove checkpoint from the queue
            cp.CheckpointReached(queueListIndex);                         // call destroy checkpoint method. May want to have animation or something later
            checkpointManager.CheckpointVisibility.UpdateVisibleCheckpoints(queueListIndex);
            thisPlayer.PlayerCheckpointsController.CheckpointGuide.UpdateGuide();       // update the checkpoint guid on the player that reached the checkpoint
            OnCheckpointReached?.Invoke();
        }

      

        /// <summary>
        /// Handles what happens if the wrong checkpoint is reached
        /// </summary>
        private void WrongCheckpoint()
        {
        }
    }
}
