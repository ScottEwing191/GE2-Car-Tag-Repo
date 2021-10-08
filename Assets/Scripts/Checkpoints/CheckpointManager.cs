using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CarTag.Checkpoints {
    public class CheckpointManager : MonoBehaviour {
        [ShowInInspector] private Queue<Checkpoint> checkpoints = new Queue<Checkpoint>();
        public Queue<Checkpoint> Checkpoints { get { return checkpoints; } }
        
        public CheckpointSpawner CheckpointSpawner { get; private set; }

        private CheckpointActivation checkpointActivation;

        private void Awake() {
            CheckpointSpawner = GetComponent<CheckpointSpawner>();
        }

    }
}
