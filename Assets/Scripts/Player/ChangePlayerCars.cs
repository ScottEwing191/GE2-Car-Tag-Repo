using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarTag.PlayerSpace;
using CarTag.Road;
using CarTag.Abilities;

namespace CarTag {
    public class ChangePlayerCars : MonoBehaviour {
        [SerializeField] private Player player;

        [Header("Car Controllers")]
        [SerializeField] public RCC_CarControllerV3 runnerCarController;
        [SerializeField] public RCC_CarControllerV3 chaserCarController;

        [Header("Rocket Ability")]
        [SerializeField] private RocketAbility rocketAbility;
        [SerializeField] private Transform runnerSpawnTransform;
        [SerializeField] private Transform chaserSpawnTransform;

        [Header("Camera")]
        [SerializeField] private RCC_Camera rcc_Camera;

        [Header("Car Rigidbodies")]
        [SerializeField] public Rigidbody runnerRigidbody;
        [SerializeField] public Rigidbody chaserRigidbody;

        private void Start() {
            // Setting up car controller settings
            //--RUNNER
            runnerCarController.maxspeed = 120;
            //--CHASER
            chaserCarController.wheelTypeChoise = RCC_CarControllerV3.WheelType.AWD;
            //chaserCarController.maxEngineTorque = 10;

        }

        public void ChangeCar(bool isNewRunner, bool enablePlayer = true) {
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

                //--Make Sure Car Changing to is enabled
                //player.EnablePlayer();
                if (enablePlayer)
                    player.EnablePlayer();
                else
                    player.DisablePlayer();

                //--Repair Car
                runnerCarController.repairNow = true;
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

                //Make Sure Car Changing to is enabled
                if (enablePlayer)
                    player.EnablePlayer();
                else
                    player.DisablePlayer();


                //--Repair Car
                chaserCarController.repairNow = true;
            }
        }
    }
}
