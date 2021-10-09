using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Checkpoints {
    public class Checkpoint : MonoBehaviour {
        public int id { get; set; }     // might come in handy
        public Vector3 respawnPosition { get; private set; }
        public Quaternion respawnRotation { get; private set; }

        [SerializeField] private GameObject mesh;
        [SerializeField] private GameObject checkpointTrigger;
        [SerializeField] private Transform respawnTransform;

        private CheckpointManager checkpointManager;

        private void Awake() {
            checkpointManager = GetComponentInParent<CheckpointManager>();
            if (respawnTransform != null) {
                respawnPosition = respawnTransform.position;
                respawnRotation = respawnTransform.rotation;
            }
            else {
                Debug.LogError("Checkpoint Respawn Transform is NULL");
            }
        }

        public void HideMeshAndCollider() {
            mesh.SetActive(false);
            checkpointTrigger.SetActive(false);
        }

        public void ShowMeshAndCollider() {
            mesh.SetActive(true);
            checkpointTrigger.SetActive(true);
        }

        public void DestroyCheckpoint() {
            Destroy(this.gameObject);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("CarTrigger")) {                                   // makes sure we are detecting the corect trigger on car
                checkpointManager.CheckpointReached.DoCheckpointReached(this, other);      // pass this checkpoint into the Checkpoint Reached Script which will determine if the 
                                                                                    // ... correct car has passed throught correct checkpoint
            }
        }

    }
}
