using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CarTag.Checkpoints {
    public class CheckpointManager : MonoBehaviour {
        [ShowInInspector] private Queue<Checkpoint> checkpoints = new Queue<Checkpoint>();
        public Queue<Checkpoint> Checkpoints { get { return checkpoints; } }
        
        public CheckpointSpawner CheckpointSpawner { get; private set; }

        public CheckpointActivation CheckpointActivation { get; set; }

        private void Awake() {
            CheckpointSpawner = GetComponent<CheckpointSpawner>();
            CheckpointActivation = new CheckpointActivation(this, 5);
        }

        private void Update() {
            if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame) {
                Checkpoints.Dequeue();
                CheckpointActivation.UpdateVisibleCheckpoints();
            }
        }

    }
}
