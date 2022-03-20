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
        [SerializeField] List<WheelCollider> wheels = new List<WheelCollider>();
        private Player player;
        private Rigidbody carRb;
        private Vector3 startPosition;          // the position of the car when the game starts. Used to reset the car at the end of the round
        private Quaternion startRotation;       // the rotation of the car when the game starts
        
        private Vector3 respawnPosition;
        private Quaternion respawnRotation;

        private void Awake() {
            player = GetComponentInParent<Player>();
        }


        private void Start() {
            carRb = player.RCC_CarController.GetComponent<Rigidbody>();
            startPosition = player.RCC_CarController.transform.position;
            startRotation = player.RCC_CarController.transform.rotation;
            SetRespawnLocation(startPosition, startRotation);
        }

        public void RespawnAtCheckpoint() {
            if (!CanRespawn()) { return; }

            SetCarTransform(respawnPosition, respawnRotation);
            SetCarVelocity(maxRespawnVelocity);
            CheckCollisionWithCars();   // must be called after the car position and rotation have changed since we want to check new position
            CheckCollisionWithLevel();
        }

        /// <summary>
        /// Respawns the cars at the given position and rotation. Sets the velocity of the cars to 0 and checks if the cars collision
        /// should be on or off
        /// </summary>
        public void RespawnAfterRoleSwap(Vector3 position, Quaternion rotation) {
            SetRespawnLocation(position, rotation);
            SetCarTransform(position, rotation);
            // For some reason respawning after Role swap was not working the way it is done after Round End. I have no clue why but...
            //... directly clamping the velocity and angular velocity seems to produce the desired (same) effects. i.e the car full stops
            carRb.velocity = Vector3.ClampMagnitude(carRb.velocity, 0);
            carRb.angularVelocity = Vector3.ClampMagnitude(carRb.angularVelocity, 0);
            StartCoroutine(StopWheelsRoleSwap());
            CheckCollisionWithCars();
        }

        /// <summary>
        /// Respawns the cars at the given position and rotation. Sets the velocity of the cars to 0.
        /// </summary>
        public void RespawnAfterRound() {
            SetRespawnLocation(startPosition, startRotation);
            SetCarTransform(startPosition, startRotation);
            //SetCarVelocity(0);
            StopCar();
            StartCoroutine(StopWheels());
        }
        /// <summary>
        /// The wheels colliders continue to spin after respawning waiting a frame before stopping them seams to fix it
        /// Problem is detailed here in solution here https://answers.unity.com/questions/35066/remove-all-forces-on-a-wheel-collider.html
        /// </summary>
        private IEnumerator StopWheels() {
            yield return new WaitForFixedUpdate();
            SetCarVelocity(0);                          // stops car from moving after respawning while on ramp
            SetWheelBrakeTorque(Mathf.Infinity);        // stops wheels from spinning, but stops acceleration from working
            yield return new WaitForFixedUpdate();
            SetWheelBrakeTorque(0);                     // allows acceleration to work again
        }

        /// <summary>
        /// The wheels colliders continue to spin after respawning waiting a frame before stopping them seams to fix it
        /// Problem is detailed here in solution here https://answers.unity.com/questions/35066/remove-all-forces-on-a-wheel-collider.html
        /// </summary>
        private IEnumerator StopWheelsRoleSwap() {
            yield return new WaitForFixedUpdate();
            //SetCarVelocity(0);                          // stops car from moving after respawning while on ramp
            SetWheelBrakeTorque(Mathf.Infinity);        // stops wheels from spinning, but stops acceleration from working
            yield return new WaitForFixedUpdate();
            SetWheelBrakeTorque(0);                     // allows acceleration to work again
        }

        //--Set the position and rotation that the car will respawn at
        public void SetRespawnLocation(Vector3 position, Quaternion rotation) {
            respawnPosition = position;
            respawnRotation = rotation;
        }
        private bool CanRespawn() {
            if (player.PlayerRoll == PlayerRoleEnum.Chaser) {
                return true;
            }
            return false;
        }

        private void SetCarTransform(Vector3 position, Quaternion rotation) {
            //--Set the position and rotation of the car to the respawn position and rotation
            player.ChangePlayerCars.runnerCarController.transform.position = position;
            player.ChangePlayerCars.runnerCarController.transform.rotation = rotation;

            player.ChangePlayerCars.chaserCarController.transform.position = position;
            player.ChangePlayerCars.chaserCarController.transform.rotation = rotation;
            /*player.RCC_CarController.transform.position = position;
            player.RCC_CarController.transform.rotation = rotation;
            if (player.PlayerListIndex == 1) {
                print(player.gameObject.name + " Set Car Position: " + position);
                print(player.gameObject.name + " Start Position: " + startPosition);
                Debug.Break();
            }*/

        }

        private void SetCarVelocity(float maxVelocity) {
            //--Set the Velocity of the car upon respon
            float velocityMagnitude = carRb.velocity.magnitude;                         // get car velocity magnitude
            Vector3 forwardVector = respawnRotation * Vector3.forward;                  // get the direction the car should move when it respawns
            Vector3 rbVelocity = forwardVector * velocityMagnitude;                     // combine car velocity magnitude with forward direction
            rbVelocity = Vector3.ClampMagnitude(rbVelocity, maxVelocity);        // clamp respawn velocity magnitude
            carRb.AddForce(-carRb.velocity, ForceMode.VelocityChange);                  // set velocity of car to zero
            carRb.AddForce(rbVelocity, ForceMode.VelocityChange);                       // set velocity of car to new value

            //--Set the Angular velocity of the car 
            carRb.AddTorque(-carRb.angularVelocity, ForceMode.VelocityChange);
        }

        private void StopCar() {
            carRb.velocity = Vector3.zero;
            carRb.angularVelocity = Vector3.zero;
        }
        private void SetWheelBrakeTorque(float torque) {
            foreach (WheelCollider wheel in wheels) {
                wheel.brakeTorque = torque;
            }
        }
        
        //--If the car is going to collide with another car at the respawn point then turn off the car's collision and start enumerator..
        //... which will turn it back on once the cars will not collide with each other
        private void CheckCollisionWithCars() {
            //--Changed line so that CarCollision Doen't need to be attached to car so I can use one car controller script instead of one for each car
            if (player.PlayerCollision.CarCollisionCheck(player.RCC_CarController.transform.position, player.RCC_CarController.transform.rotation)) {
                player.PlayerCollision.SetGameObjectListToLayer("Car No Collision");                    // turn off car collision
                StartCoroutine(player.PlayerCollision.TurnOnCarCollision(0.1f));  // start enumerator to turn it back on
            }
        }

        private void CheckCollisionWithLevel() {
            //throw new NotImplementedException();
        }
    }
}
