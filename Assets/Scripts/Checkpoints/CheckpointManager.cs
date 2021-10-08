using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Checkpoints
{
    public class CheckpointManager : MonoBehaviour
    {
        // Not having new Queue might cause problems
        public Queue<Checkpoint> Checkpoints { get; set; }
        public CheckpointSpawner CheckpointSpawner { get; private set; }

        private void Awake() {
            CheckpointSpawner = GetComponent<CheckpointSpawner>();
        }
        
    }
}
