using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.PlayerSpace;
using CarTag.Road;
using CarTag.Abilities;

namespace CarTag {
    public class ChangePlayerCars : MonoBehaviour {
        [SerializeField] private Player player;

        /*[Header("Car Game Objects")]
        [SerializeField] private GameObject runnerCar;
        [SerializeField] private GameObject chaserCar;*/

        /*[Header ("Player Collision")]
        [SerializeField] private PlayerCollision runnerCollisionScript;
        [SerializeField] private PlayerCollision chaserCollisionScript;*/

        /*[Header("Player Respawn")]
        [SerializeField] private PlayerRespawn runnerRespawnScript;
        [SerializeField] private PlayerRespawn chaserRespawnScript;*/

        /*[Header("Road Spawn Data ")]
        [SerializeField] private RoadSpawnData roadSpawnData;
        [SerializeField] private FollowCar roadSpawnDataFollowCar;
        [SerializeField] private List<WheelCollider> runnerRearWheels = new List<WheelCollider>();
        [SerializeField] private List<WheelCollider> chaserRearWheels = new List<WheelCollider>();*/

        [Header("Rocket Ability")]
        [SerializeField] private RocketAbility rocketAbility;
        [SerializeField] private Transform runnerSpawnTransform;
        [SerializeField] private Transform chaserSpawnTransform;

        /*[Header("Box Spawn Ability")]
        [SerializeField] private FollowCar boxSpawnFollowCar;
        [SerializeField] private Transform runnerTargetCar;
        [SerializeField] private Transform chaserTargetCar;*/

        [Header("Camera")]
        [SerializeField] private RCC_Camera rcc_Camera;
        [SerializeField] private RCC_CarControllerV3 runnerCarController;
        [SerializeField] private RCC_CarControllerV3 chaserCarController;

        [Header("Car Rigidbodies")]
        [SerializeField] private Rigidbody runnerRigidbody;
        [SerializeField] private Rigidbody chaserRigidbody;


        public void ChangeCar(bool isNewRunner) {
            //-- If New Runner i.e changing to runner
            if (isNewRunner) {
                player.RCC_CarController = runnerCarController;
                rcc_Camera.playerCar = runnerCarController;
                
                //--Set Car Object Active
                runnerCarController.gameObject.SetActive(true);
                chaserCarController.gameObject.SetActive(false);

                //--Set Car Position to the position of the other car
                runnerCarController.transform.position = chaserCarController.transform.position;
                runnerCarController.transform.rotation = chaserCarController.transform.rotation;

                //--Get Current Velocity from other car
                Vector3 currentVelocity = chaserRigidbody.velocity;
                Vector3 currentAngularVelocity = chaserRigidbody.angularVelocity;

                //--Set Current Velocity of new car to velocity of other car
                runnerRigidbody.AddForce(currentVelocity, ForceMode.VelocityChange);
                runnerRigidbody.AddTorque(currentAngularVelocity, ForceMode.VelocityChange);

                //--Set Rocket Spawn Transform
                rocketAbility.SpawnTransform = runnerSpawnTransform;

            }
            //-- If New Chaser i.e changing to chaser
            else {
                player.RCC_CarController = chaserCarController;
                rcc_Camera.playerCar = chaserCarController;


                //--Set Car Object Active
                runnerCarController.gameObject.SetActive(false);
                chaserCarController.gameObject.SetActive(true);

                //--Set Car Position to the position of the other car
                chaserCarController.transform.position = runnerCarController.transform.position;
                chaserCarController.transform.rotation = runnerCarController.transform.rotation;

                //--Get Current Velocity from other car
                Vector3 currentVelocity = runnerRigidbody.velocity;
                Vector3 currentAngularVelocity = runnerRigidbody.angularVelocity;

                //--Set Current Velocity of new car to velocity of other car
                chaserRigidbody.AddForce(currentVelocity, ForceMode.VelocityChange);
                chaserRigidbody.AddTorque(currentAngularVelocity, ForceMode.VelocityChange);

                //--Set Rocket Spawn Transform
                rocketAbility.SpawnTransform = chaserSpawnTransform;
            }


        }





    }
}
