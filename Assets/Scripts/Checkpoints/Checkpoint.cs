using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Checkpoints
{
    public class Checkpoint : MonoBehaviour
    {
        public int id { get; set; }     // might come in handy
        public Vector3 respawnPosition { get; private set; }
        public Quaternion respawnRotation { get; private set; }
        
        [SerializeField] private GameObject mesh;
        [SerializeField] private GameObject collider;
        [SerializeField] private Transform respawnTransform;

        private void Awake() {
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
            collider.SetActive(false);
        }

        public void ShowMeshAndCollider() {
            mesh.SetActive(true);
            collider.SetActive(true);
        }
    }
}
