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
        [SerializeField] private GameObject greenParticles;
        [SerializeField] private GameObject yellowParticles;
        [SerializeField] private GameObject redParticles;


        public bool IsCheckpointReached { get; set; }

        //Properties
        public GameObject Mesh { get { return mesh; } }
        public GameObject CheckpointTrigger { get { return checkpointTrigger; } }
        public GameObject GreenParticles { get { return greenParticles; } }
        public GameObject YellowParticles { get { return yellowParticles; } }
        public GameObject RedParticles { get { return redParticles; } }

        // Constructor
        /*public CheckpointData(GameObject mesh, GameObject checkpointTrigger) {
            this.mesh = mesh;
            this.checkpointTrigger = checkpointTrigger;
            this.IsCheckpointReached = false;
        }*/

        public CheckpointData(GameObject mesh, GameObject checkpointTrigger, GameObject greenParticle, GameObject yellowParticle, GameObject redParticle) {
            this.mesh = mesh;
            this.checkpointTrigger = checkpointTrigger;
            this.greenParticles = greenParticle;
            this.yellowParticles = yellowParticle;
            this.redParticles = redParticle;
            this.IsCheckpointReached = false;
        }
    }

    public class Checkpoint : MonoBehaviour {
        //--Serialized Fields
        [SerializeField] private List<CheckpointData> playerCheckpoints = new List<CheckpointData>();
        [SerializeField] private Transform respawnTransform;
        /*[SerializeField] private GameObject greenParticles;
        [SerializeField] private GameObject yellowParticles;
        [SerializeField] private GameObject redParticles;*/


        //--Auto-Implemented Properties
        public int id { get; set; }     // might come in handy
        public Vector3 respawnPosition { get; private set; }
        public Quaternion respawnRotation { get; private set; }

        //--Private
        private CheckpointManager checkpointManager;

        private void Awake() {
            if (respawnTransform != null) {
                respawnPosition = respawnTransform.position;
                respawnRotation = respawnTransform.rotation;
            }
            else {
                Debug.LogError("Checkpoint Respawn Transform is NULL");
            }
            //REMINDER: This can be set in awake since checkpoints are only spawned once the game has already begun
            checkpointManager = GameManager.Instance.CheckpointManager;
            RemoveUnusedCheckpointDatas();
            //SetupPlayerCheckpointsList();
        }

        //--The Checkpoint data's will all be set up in the prefab. This method will remove the one's from the list which are not required given the number...
        //... of players
        private void RemoveUnusedCheckpointDatas() {
            int playerInGame = checkpointManager.CheckpointQueues.Count;
            int cpDatasToRemove = playerCheckpoints.Count - playerInGame;                         
            for (int i = 0; i < cpDatasToRemove; i++) {
                int dataIndexToRemove = playerCheckpoints.Count - 1;
                playerCheckpoints[dataIndexToRemove].Mesh.transform.parent.gameObject.SetActive(false);     //Disables the Game Object which is not required.
                playerCheckpoints.RemoveAt(dataIndexToRemove);    //Removes the last player from the list

            }
        }

        /// <summary>
        /// The prefab checkpoint has a mesh and a trigger for each of the players(up to 4 so for now). This methods finds them and creates aCheckpointData for them and adds it to a ...
        /// list. if there are less than 4 players in the game then less than 4 CheckpointData's will be made and the extra game objects in the prefab will be disabled.
        /// </summary>
        /*private void SetupPlayerCheckpointsList() {
            foreach (Transform child in transform) {
                if (child.gameObject.CompareTag("CPMeshAndTrigger")) {                              // If the child has the correct tag

                    if (playerCheckpoints.Count < checkpointManager.CheckpointQueues.Count) {       // Has a checkpoint checkpoint data been added for each player in the game 
                        CheckpointData data = new CheckpointData(child.GetChild(0).gameObject, child.GetChild(1).gameObject);
                        playerCheckpoints.Add(data);
                    }
                    else {
                        child.gameObject.SetActive(false);
                    }
                }
            }
        }*/

        

        //=== PUBLIC METHODS ===
        public void HideCheckpointForPlayer(int playerIndex) {
            HideMeshAndCollider(playerCheckpoints[playerIndex]);
        }
        public void ShowCheckpointForPlayer(int playerIndex) {
            ShowMeshAndCollider(playerCheckpoints[playerIndex]);
        }

        public void SetColourOfCheckpoint(int cpNumber, int playerIndex) {
            switch (cpNumber) {
                case 0:                 // Green Checkpoint
                    playerCheckpoints[playerIndex].GreenParticles.SetActive(true);
                    playerCheckpoints[playerIndex].RedParticles.SetActive(false);
                    playerCheckpoints[playerIndex].YellowParticles.SetActive(false);
                    break;
                case 1:                 // Yellow Checkpoint
                    playerCheckpoints[playerIndex].GreenParticles.SetActive(false);
                    playerCheckpoints[playerIndex].RedParticles.SetActive(true);
                    playerCheckpoints[playerIndex].YellowParticles.SetActive(false);
                    break;
                case 2:                 // Red Checkpoint
                    playerCheckpoints[playerIndex].GreenParticles.SetActive(false);
                    playerCheckpoints[playerIndex].RedParticles.SetActive(false);
                    playerCheckpoints[playerIndex].YellowParticles.SetActive(true);
                    break;
                default:                // Red Checkpoint
                    playerCheckpoints[playerIndex].GreenParticles.SetActive(false);
                    playerCheckpoints[playerIndex].RedParticles.SetActive(false);
                    playerCheckpoints[playerIndex].YellowParticles.SetActive(true);
                    break;
            }
        }
        public void CheckpointReached(int playerCheckpointsIndex) {
            //--Get the Checkpoint data that has just been collided with using the playerCheckpointsIndex and set the IsCheckpointReached value to true
            CheckpointData d = playerCheckpoints[playerCheckpointsIndex];
            d.IsCheckpointReached = true;
            playerCheckpoints[playerCheckpointsIndex] = d;
            HideCheckpointForPlayer(playerCheckpointsIndex);

            //--Check if every player has reached this checkpoint and if so call the destroy Checkpoint method
            for (int i = 0; i < playerCheckpoints.Count; i++) {
                //--Dont need to worry if the Current runner has reached any checkpoints
                if (GameManager.Instance.PlayerManager.CurrentRunner.PlayerListIndex == i) {
                    continue;
                }
                if (playerCheckpoints[i].IsCheckpointReached == false) { return; }
            }
            DestroyCheckpoint();
        }

        //=== PRIVATE METHODS ===

        private void HideMeshAndCollider(CheckpointData data) {
            data.Mesh.SetActive(false);
            data.CheckpointTrigger.SetActive(false);
        }

        private void ShowMeshAndCollider(CheckpointData data) {
            data.Mesh.SetActive(true);
            data.CheckpointTrigger.SetActive(true);
        }

        

        private void DestroyCheckpoint() {
            //--Initiate the removal of the section of road before this checkpoint
            GameManager.Instance.RoadManager.RemoveRoad(transform.position);
            Destroy(this.gameObject);
        }


        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("CarTrigger")) {                                   // makes sure we are detecting the corect trigger on car
                //--pass this checkpoint and colliding player into the Checkpoint Reached Script which will determine if the correct player has collided 
                //--with the correct checkpoint 
                checkpointManager.HandleCheckpointReached(this, checkpointManager.PlayerManager.GetPlayerFromGameObject(other.gameObject));
            }
        }
    }
}
