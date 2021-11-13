using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Checkpoints {
    /// <summary>
    /// Controls how many checkpoints are visible at once and which ones. No point in having 20 visible checkpoints when chaser can only focus on a few at a time
    /// </summary>
    public class CheckpointVisibility {
        private CheckpointManager checkpointManager;
        private int visibleCheckpoints;
        public CheckpointVisibility(CheckpointManager checkpointManager, int visibleAtOnce) {
            this.visibleCheckpoints = visibleAtOnce;
            this.checkpointManager = checkpointManager;
        }
        internal void SetNewCheckpointVisibility(Checkpoint checkpoint, int currentRunnerIndex) {
            // For every queue in the List (except the runner queue) Check how many Checkpoints are in the queue and count is greater than the number of checkpoints which...
            // ... should be visible at one time then hide the mesh on the checkpoint that corresponds to the current queue. So separate chaser players see different ...
            // ... checkpoints depending on their progress
            for (int i = 0; i < checkpointManager.CheckpointQueues.Count; i++) {
                if (i == currentRunnerIndex) {
                    checkpoint.HideCheckpointForPlayer(i);      // Always hide the checkpoint from the runner
                    continue;
                }
                //--if the current players queue has more checkpoints than the number of chekpoints which can be visible at once then hide the new... 
                //...checkpoint for the current player
                if (checkpointManager.CheckpointQueues[i].Count > visibleCheckpoints) {
                    checkpoint.HideCheckpointForPlayer(i);
                }
            }
        }

        //--Makes sure that all the checkpoints which should be visible are
        //--2+ player Version
        public void UpdateVisibleCheckpoints(int queueIndex) {
            if (checkpointManager.CheckpointQueues[queueIndex].Count == 0) { return; }
            int loopLimit = visibleCheckpoints;

            //--does same as line below but more confusing to read
            //loopLimit = checkpointManager.CheckpointQueues[queueIndex].Count >= visibleCheckpoints ? visibleCheckpoints : checkpointManager.CheckpointQueues[queueIndex].Count;
            //--clamp the no. of cp's in the queue between 0 and the no. cp's which are allowed to be visible. Use this no. as the limit for the loop
            loopLimit = Mathf.Clamp(checkpointManager.CheckpointQueues[queueIndex].Count, 0, visibleCheckpoints);
            var checkpointsAsArray = checkpointManager.CheckpointQueues[queueIndex].ToArray();
            for (int i = 0; i < loopLimit; i++) {
                checkpointsAsArray[i].ShowCheckpointForPlayer(queueIndex);
                //--Set Checkpoint Colour
            }
        }
    }
}
