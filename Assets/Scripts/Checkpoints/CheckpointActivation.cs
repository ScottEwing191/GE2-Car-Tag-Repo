using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Checkpoints
{
    public class CheckpointActivation
    {
        private CheckpointManager checkpointManager;
        private int activeAtOnce = 5;
        public CheckpointActivation(CheckpointManager checkpointManager, int acitiveAtOnce) {
            this.activeAtOnce = activeAtOnce;
            this.checkpointManager = checkpointManager;
        }


    }
}
