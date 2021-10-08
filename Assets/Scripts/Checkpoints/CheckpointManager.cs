using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace CarTag.Checkpoints {
    public class CheckpointManager : MonoBehaviour {
        [ShowInInspector] private Queue<Checkpoint> checkpoints = new Queue<Checkpoint>();
        [SerializeField] private int visibleCheckpoints = 5;
        public Queue<Checkpoint> Checkpoints { get { return checkpoints; } }

        public CheckpointSpawner CheckpointSpawner { get; private set; }

        public CheckpointActivation CheckpointVisibility { get; set; }

        private void Awake() {
            CheckpointSpawner = GetComponent<CheckpointSpawner>();
            CheckpointVisibility = new CheckpointActivation(this, visibleCheckpoints);
        }

        private void Update() {
            if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame && checkpoints.Count > 0) {
                Destroy(Checkpoints.Dequeue().gameObject);
                CheckpointVisibility.UpdateVisibleCheckpoints();
            }
        }
        /// <summary>
        /// Takes in the transform where a checkpoint it to be spawned. Tells Checkpoint Spawner to try and spawn checkpoint
        /// if sucessful, tells CheckpointVisibility to set the visibility of the checkpoint appropriately
        /// </summary>
        /// <param name="roadSpawnDataTransform"></param>
        internal void StartCheckpointSpawn(Transform roadSpawnDataTransform) {
            var newCPScript = CheckpointSpawner.TrySpawnCheckpoint(roadSpawnDataTransform);
            if (newCPScript != null) {
                CheckpointVisibility.SetNewCheckpointVisibility(newCPScript);
            }
        }
    }
}
