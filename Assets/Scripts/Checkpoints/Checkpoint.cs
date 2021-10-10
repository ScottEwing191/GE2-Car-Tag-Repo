using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Linq;

namespace CarTag.Checkpoints {
    [System.Serializable]
    public struct CheckpointData {
        [SerializeField] private GameObject mesh;
        [SerializeField] private GameObject checkpointTrigger;
        public bool IsCheckpointReached { get; set; }

        //Properties
        public GameObject Mesh { get { return mesh; } }
        public GameObject CheckpointTrigger { get { return checkpointTrigger; } }
        /*public bool IsCheckpointReached { 
            get { return isCheckpointReached; } 
            set { isCheckpointReached = value; }
        }*/

        // Constructor
        public CheckpointData(GameObject mesh, GameObject checkpointTrigger) {
            this.mesh = mesh;
            this.checkpointTrigger = checkpointTrigger;
            this.IsCheckpointReached = false;
        }
    }

    public class Checkpoint : MonoBehaviour {
        public int id { get; set; }     // might come in handy
        public Vector3 respawnPosition { get; private set; }
        public Quaternion respawnRotation { get; private set; }

        [SerializeField] private GameObject mesh;
        [SerializeField] private GameObject checkpointTrigger;
        [SerializeField] private List<CheckpointData> playerCheckpoints = new List<CheckpointData>();
        [SerializeField] private Transform respawnTransform;

        private CheckpointManager checkpointManager;

        private void Awake() {
            if (respawnTransform != null) {
                respawnPosition = respawnTransform.position;
                respawnRotation = respawnTransform.rotation;
            }
            else {
                Debug.LogError("Checkpoint Respawn Transform is NULL");
            }
            checkpointManager = GameManager.Instance.CheckpointManager;
            SetupPlayerCheckpointsList();
        }
        /// <summary>
        /// The prefab checkpoint has a mesh and a trigger for each of the players(up to 4 so for now). This methods finds them and creates aCheckpointData for them and adds it to a ...
        /// list. if there are less than 4 players in the game then less than 4 CheckpointData's will be made and the extra game objects in the prefab will be disabled.
        /// </summary>
        private void SetupPlayerCheckpointsList() {
            foreach (Transform child in transform) {
                if (child.gameObject.CompareTag("CPMeshAndTrigger")) {      // If the child has the correct tag

                    if (playerCheckpoints.Count < checkpointManager.CheckpointQueues.Count) {       // Has a checkpoint checkpoint data been added for each player in the game 
                        CheckpointData data = new CheckpointData(child.GetChild(0).gameObject, child.GetChild(1).gameObject);
                        playerCheckpoints.Add(data);
                    }
                    else {
                        child.gameObject.SetActive(false);
                    }
                }
            }
        }

        public void HideCheckpointForPlayer(int playerIndex) {
            HideMeshAndCollider(playerCheckpoints[playerIndex]);
        }
        public void ShowCheckpointForPlayer(int playerIndex) {
            ShowMeshAndCollider(playerCheckpoints[playerIndex]);
        }

        private void HideMeshAndCollider(CheckpointData data) {
            data.Mesh.SetActive(false);
            data.CheckpointTrigger.SetActive(false);
        }

        private void ShowMeshAndCollider(CheckpointData data) {
            data.Mesh.SetActive(true);
            data.CheckpointTrigger.SetActive(true);
        }

        private void DestroyCheckpoint() {
            Destroy(this.gameObject);
        }

        public void CheckpointReached(int playerCheckpointsIndex) {
            // Gett the Checkpoint data that has just been collided with using the playerCheckpointsIndex and set the IsCheckpointReached value to true
            CheckpointData d = playerCheckpoints[playerCheckpointsIndex];
            d.IsCheckpointReached = true;
            playerCheckpoints[playerCheckpointsIndex] = d;
            HideCheckpointForPlayer(playerCheckpointsIndex);

            //Check if every player has reached this checkpoint and if so call the destroy Checkpoint method
            for (int i = 0; i < playerCheckpoints.Count; i++) {
                //Dont need to worry if the Current runner has reached any checkpoints
                if (GameManager.Instance.PlayerManager.CurrentRunner.PlayerListIndex == i) {
                    continue;
                }
                if (playerCheckpoints[i].IsCheckpointReached == false) { return; }
            }
            DestroyCheckpoint();
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("CarTrigger")) {                                   // makes sure we are detecting the corect trigger on car
                // pass this checkpoint and colliding player into the Checkpoint Reached Script which will determine if the correct player has collided with the correct checkpoint 
                checkpointManager.CheckpointReached.DoCheckpointReached(this, GameManager.Instance.PlayerManager.GetPlayerFromGameObject(other.gameObject));
            }
        }
    }
}
