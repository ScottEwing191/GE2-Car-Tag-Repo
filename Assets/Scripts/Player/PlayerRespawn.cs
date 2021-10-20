using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.Checkpoints;
using System;

namespace CarTag.PlayerSpace
{
    public class PlayerRespawn : MonoBehaviour
    {
        [SerializeField] float maxRespawnVelocity = 25;
        [SerializeField] LayerMask collisionOffCheckMask;
        [SerializeField] LayerMask collisionOnCheckMask;

        private Player player;
        private Rigidbody carRb;
        private Vector3 respawnPosition;
        private Quaternion respawnRotation;

        private void Awake() {
            player = GetComponentInParent<Player>();
            carRb = GetComponent<Rigidbody>();
        }

        public void RespawnAtCheckpoint() {
            SetCarTransform(respawnPosition, respawnRotation);
            SetCarVelocity();
            
            CheckCollisionWithCars();   // must be called after the car position and rotation have changed since we want to check new position
            CheckCollisionWithLevel();
        }

        public void RespawnAfterRoleSwap(Vector3 position, Quaternion rotation) {
            SetCarTransform(position, rotation);
            SetCarVelocity();
            CheckCollisionWithCars();
        }

        private void SetCarTransform(Vector3 position, Quaternion rotation) {
            //--Set the position and rotation of the car to the respawn position and rotation
            carRb.transform.position = position;
            carRb.transform.rotation = rotation;
        }

        private void SetCarVelocity() {
            //--Set the Velocity of the car upon respon
            float velocityMagnitude = carRb.velocity.magnitude;                         // get car velocity magnitude
            Vector3 forwardVector = respawnRotation * Vector3.forward;                  // get the direction the car should move when it respawns
            Vector3 rbVelocity = forwardVector * velocityMagnitude;                     // combine car velocity magnitude with forward direction
            rbVelocity = Vector3.ClampMagnitude(rbVelocity, maxRespawnVelocity);        // clamp respawn velocity magnitude
            carRb.AddForce(-carRb.velocity, ForceMode.VelocityChange);                  // set velocity of car to zero
            carRb.AddForce(rbVelocity, ForceMode.VelocityChange);                       // set velocity of car to new value

            //--Set the Angular velocity of the car 
            carRb.AddRelativeTorque(-carRb.angularVelocity, ForceMode.VelocityChange);  // remove angular velocity from car when it respawns
        }
        
        //--If the car is going to collide with another car at the respawn point then turn off the car's collision and start enumerator..
        //... which will turn it back on once the cars will not collide with each other
        private void CheckCollisionWithCars() {
            if (player.PlayerCollision.CarCollisionCheck(collisionOffCheckMask)) {
                player.PlayerCollision.SetGameObjectListToLayer("Car No Collision");                    // turn off car collision
                StartCoroutine(player.PlayerCollision.TurnOnCarCollision(0.1f, collisionOnCheckMask));  // start enumerator to turn it back on
            }
        }


        private void CheckCollisionWithLevel() {
            //throw new NotImplementedException();
        }


        private void OnTriggerEnter(Collider other) {
            //--When going through checkpoint record the respawn point atached to checkpoint 
            if (other.gameObject.CompareTag("Checkpoint")) {
                print("Player Collided With Trigger");
                Checkpoint cp = other.gameObject.GetComponentInParent<Checkpoint>();
                respawnPosition = cp.respawnPosition;
                respawnRotation = cp.respawnRotation;
            }
        }
    }
}
