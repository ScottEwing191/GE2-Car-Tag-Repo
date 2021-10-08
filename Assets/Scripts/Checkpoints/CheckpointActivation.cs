using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Checkpoints
{
    /// <summary>
    /// Controls how many checkpoints are visible at once and which ones. No point in having 20 visible checkpoints when chaser can only focus on a few at a time
    /// </summary>
    public class CheckpointActivation
    {
        private CheckpointManager checkpointManager;
        private int activeAtOnce;
        public CheckpointActivation(CheckpointManager checkpointManager, int activeAtOnce) {
            this.activeAtOnce = activeAtOnce;
            this.checkpointManager = checkpointManager;
            Debug.Log("Constructor");
        }

        /// <summary>
        /// When a new checkpoint is added it decides whether it should be visible or not
        /// </summary>
        public void SetNewCheckpointVisibility(Checkpoint checkpoint) {
            if (checkpointManager.Checkpoints.Count > activeAtOnce) {
                checkpoint.HideMeshAndCollider();
            }
        }
        /// <summary>
        /// Makes sure that all the checkpoints which should be visible are
        /// </summary>
        public void UpdateVisibleCheckpoints() {
            int index = checkpointManager.Checkpoints.Count - 1;
            int lowerBound = (checkpointManager.Checkpoints.Count - 1) - activeAtOnce;
            var checkpointAsArray = checkpointManager.Checkpoints.ToArray();
            for (int i = index; i > lowerBound; i--) {
                checkpointAsArray[i].ShowMeshAndCollider();
            }
            
        }

    }
}
