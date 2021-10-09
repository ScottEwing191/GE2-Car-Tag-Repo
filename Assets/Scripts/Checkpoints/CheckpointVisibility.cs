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
        public CheckpointVisibility(CheckpointManager checkpointManager, int activeAtOnce) {
            this.visibleCheckpoints = activeAtOnce;
            this.checkpointManager = checkpointManager;
        }

        /// <summary>
        /// When a new checkpoint is added it decides whether it should be visible or not
        /// </summary>
        public void SetNewCheckpointVisibility(Checkpoint checkpoint) {
            if (checkpointManager.Checkpoints.Count > visibleCheckpoints) {
                checkpoint.HideMeshAndCollider();
            }
        }
        /// <summary>
        /// Makes sure that all the checkpoints which should be visible are
        /// </summary>
        public void UpdateVisibleCheckpoints() {
            if (checkpointManager.Checkpoints.Count == 0) { return; }
            int loopLimit = visibleCheckpoints;

            loopLimit = checkpointManager.Checkpoints.Count >= visibleCheckpoints ? visibleCheckpoints : checkpointManager.Checkpoints.Count;

            var checkpointAsArray = checkpointManager.Checkpoints.ToArray();
            for (int i = 0; i < loopLimit; i++) {
                checkpointAsArray[i].ShowMeshAndCollider();
            }
        }

    }
}
