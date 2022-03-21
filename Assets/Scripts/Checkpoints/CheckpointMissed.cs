using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Checkpoints {
    public class CheckpointMissed : MonoBehaviour {
        //--Serialised Fields
        [SerializeField] float thresholdDst = 2;

        //--Private
        private float _currentDst;
        private float _minimumDst;
        bool isDisplayingHint = false;

        //--Auto-Implemented Properties
        public Checkpoint TargetCheckpoint { get; set; }
        public PlayerCheckpointsController PlayerCheckpointsController { get; set; }

        private void Awake() {
            PlayerCheckpointsController = GetComponentInParent<PlayerCheckpointsController>();
        }

        private void OnEnable() {
            CheckpointReached.OnCheckpointReached += OnCheckpointReached;
            FindObjectOfType<CheckpointSpawner>().OnCheckpointSpawned -= OnCheckpointSpawned;       // I dont like this...
        }
        private void OnDisable() {
            CheckpointReached.OnCheckpointReached -= OnCheckpointReached;
            FindObjectOfType<CheckpointSpawner>().OnCheckpointSpawned -= OnCheckpointSpawned;       // ...or this
        }

        public void UpdateTarget() {
            if (PlayerCheckpointsController.ThisPlayer.IsThisPlayerCurrentRunner()) { return; }

            
            TargetCheckpoint = PlayerCheckpointsController.CheckpointsQueue.Peek();
            //TargetCheckpoint = PlayerCheckpointsController.CheckpointsQueue.ToArray()[0];
        }
            
        //--Will keep track of how close the car is to the target checkpoint and if it start to get further away from the checkpoint it will display a hint to
        //--the player. The Hint disapears once the player get closer to the Target checkpoint again
        private void FixedUpdate() {
            //print(PlayerCheckpointsController.CheckpointsQueue.Count);
            if (TargetCheckpoint == null) { return; }
            Vector3 carPosition = PlayerCheckpointsController.ThisPlayer.RCC_CarController.transform.position;
            _currentDst = Vector3.Distance(carPosition, TargetCheckpoint.transform.position);
            _minimumDst = Mathf.Min(_minimumDst, _currentDst);
            if (Mathf.Abs(_currentDst - _minimumDst) > thresholdDst && !isDisplayingHint) {
                //--Display Hint
                print("Show Hint");
                isDisplayingHint = true;
            }
            else if (isDisplayingHint) {
                //--Hide Hint
                print("Hide Hint");
                isDisplayingHint = false;
            }
        }

        private void OnCheckpointReached() {
            UpdateTarget();
        }

        private void OnCheckpointSpawned() {
            UpdateTarget();
        }

    }
}
