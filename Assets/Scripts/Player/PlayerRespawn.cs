using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Checkpoints;

namespace CarTag.PlayerSpace
{
    public class PlayerRespawn : MonoBehaviour
    {
        [SerializeField] float maxRespawnVelocity = 25;
        private Rigidbody carRb;
        private Vector3 respawnPosition;
        private Quaternion respawnRotation;

        private void Awake() {
            carRb = GetComponent<Rigidbody>();
        }

        private void Update() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.P)) {
                RespawnAtCheckpoint();
            }
        }
        public void RespawnAtCheckpoint() {
            carRb.transform.position = respawnPosition;
            carRb.transform.rotation = respawnRotation;

            float velocityMagnitude = carRb.velocity.magnitude;
            Vector3 forwardVector = respawnRotation * Vector3.forward;
            carRb.AddRelativeTorque(-carRb.angularVelocity, ForceMode.VelocityChange);
            Vector3 rbVelocity= forwardVector * velocityMagnitude;
            rbVelocity = Vector3.ClampMagnitude(rbVelocity, maxRespawnVelocity);

            carRb.AddForce(-carRb.velocity, ForceMode.VelocityChange);
            carRb.AddForce(rbVelocity, ForceMode.VelocityChange);


        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("Checkpoint")) {
                print("Player Collided With Trigger");
                Checkpoint cp = other.gameObject.GetComponentInParent<Checkpoint>();
                respawnPosition = cp.respawnPosition;
                respawnRotation = cp.respawnRotation;
            }
        }
    }
}
